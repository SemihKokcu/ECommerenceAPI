using ECommerenceAPI.Application.Abstractions.Services;
using ECommerenceAPI.Application.Abstractions.Token;
using ECommerenceAPI.Application.DTOs;
using ECommerenceAPI.Application.DTOs.Facebook;
using ECommerenceAPI.Application.Exceptions;
using ECommerenceAPI.Application.Features.Commands.AppUser.LoginUser;
using ECommerenceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance.Services
{
    public class AuthService : IAuthService
    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly IUserService _userService;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<Token> FacebookLoginAsync(string authToken, int tokenLifeTime)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");
            FacebookAccessTokenResponse? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);
            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse?.AccessToken}");

            FacebookUserAccessTokenValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);

            if (validation?.Data.IsValid != null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

                FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

                var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");


                Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
               
                return await CreateUserExternalAsync(user, userInfo.Email,userInfo.Name,info,tokenLifeTime);
            
            }
            throw new Exception("Invalid Exrernal Authentication");


        }

        public async Task<Token> GoogleLoginAsync(string idToken, int tokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:Client_ID"] }
            };

            // bu verileri karşılaştırdık
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            // kullanıcının bizde dış taraftan geldiği için ayrı bir tablo da dış kaynakk olarak tutuypruz
            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");


            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            
            return await CreateUserExternalAsync(user, payload.Email,payload.Name,info,tokenLifeTime);

        }

        public async Task<Token> LoginAsync(string UserNameOrEmail, string password, int tokenLifeTime)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(UserNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(UserNameOrEmail);

            if (user == null)
                throw new DirectoryNotFoundException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded) // auth success
            {
                Token token = _tokenHandler.CreateAccessToken(tokenLifeTime,user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15);

                return token;
                
            }
            
            throw new AuthenticationErrorExeption();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u=>u.RefreshToken == refreshToken);
            if (user !=null && user.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(15,user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user,token.Expiration,15);
                return token;
            }
            else
            {
                throw new NotFoundUserException();
            }
        }

        async Task<Token> CreateUserExternalAsync(AppUser user,string email,string name,UserLoginInfo info,int tokenLifeTime)
        {
                bool result = user != null;
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = email,
                            UserName = email,
                            NameSurname = name,

                        };
                        var identityResult = await _userManager.CreateAsync(user);
                        result = identityResult.Succeeded;

                    }
                }

                if (result)
                {
                    await _userManager.AddLoginAsync(user, info);

                    Token token = _tokenHandler.CreateAccessToken(tokenLifeTime, user);
                    await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration,15);
                    return token;
                }
             throw new Exception("Invalid Exrernal Authentication");
        }
    }
}

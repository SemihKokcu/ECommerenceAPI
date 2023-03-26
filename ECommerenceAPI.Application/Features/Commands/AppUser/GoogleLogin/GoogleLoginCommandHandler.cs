using ECommerenceAPI.Application.Abstractions.Token;
using ECommerenceAPI.Application.DTOs;
using ECommerenceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            this._userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            //google login için angular tarafında sağlamış olduğumuz key tamınladık
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { "38728492749-gnqavc7ntjqdda7cgnqd01ok1tnsno4v.apps.googleusercontent.com" }
            };

            // bu verileri karşılaştırdık
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            // kullanıcının bizde dış taraftan geldiği için ayrı bir tablo da dış kaynakk olarak tutuypruz
            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);


            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            bool result = user != null;
            if (user == null) 
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user ==null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        NameSurname = payload.Name,

                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;

                }
            }

            if (result)

                await _userManager.AddLoginAsync(user, info);
            else
                throw new Exception("Invalid Exrernal Authentication");

            Token token =  _tokenHandler.CreateAccessToken(5);
            return new()
            {
                Token = token,
            };

        }
    }
}

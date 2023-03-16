using ECommerenceAPI.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int minute)
        {
            Application.DTOs.Token token = new ();

            // security key symmetric alıdnı
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //şifrelenmiş kimliği oluşturuyoruz
            SigningCredentials signingCredentials = new(securityKey,SecurityAlgorithms.HmacSha256);

            //token ayrlarınız veriyoruz
            token.Expiration = DateTime.UtcNow.AddMinutes(minute);
            JwtSecurityToken jwtSecurityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore:DateTime.UtcNow,
                signingCredentials:signingCredentials
                );

            //Token oluşturucus sınıfından bir örnek alalım
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);

            return token;

        }
    }
}

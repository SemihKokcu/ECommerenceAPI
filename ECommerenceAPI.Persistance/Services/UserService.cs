using ECommerenceAPI.Application.Abstractions.Services;
using ECommerenceAPI.Application.DTOs.User;
using ECommerenceAPI.Application.Features.Commands.AppUser.CreateUser;
using ECommerenceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                NameSurname = model.NameSurname,
                Email = model.Email,

            }, model.Password);

            CreateUserResponse response = new CreateUserResponse() { Succeded = result.Succeeded };
            if (result.Succeeded)
                response.Message = "Kullanıcı oluşturuldu";
            else
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}\n";
                }
            return response;
        }
    }
}

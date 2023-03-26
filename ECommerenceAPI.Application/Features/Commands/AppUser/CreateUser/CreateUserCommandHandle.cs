using ECommerenceAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U = ECommerenceAPI.Domain.Entities.Identity;
namespace ECommerenceAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandle : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<U.AppUser> _userManager;
        public CreateUserCommandHandle(UserManager<U.AppUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result =  await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                NameSurname = request.NameSurname,
                Email = request.Email,

            },request.Password);
            CreateUserCommandResponse response = new CreateUserCommandResponse() { Succeded=result.Succeeded};
            if (result.Succeeded)
                response.Message = "Kullanıcı oluşturuldu";
            else
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}\n";
                }
            return response;
              

            //throw new UserCreateFailException();

        }
    }
}

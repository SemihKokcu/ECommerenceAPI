using ECommerenceAPI.Application.Abstractions.Services;
using ECommerenceAPI.Application.DTOs.User;
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
        readonly IUserService _userService;

        public CreateUserCommandHandle(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {

           CreateUserResponse response =  await  _userService.CreateAsync(new()
            {
                Email = request.Email,
                NameSurname = request.NameSurname,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                UserName = request.UserName
            });

            return new()
            {
                Message = response.Message,
                Succeded = response.Succeded,
            };
            //throw new UserCreateFailException();

        }
    }
}

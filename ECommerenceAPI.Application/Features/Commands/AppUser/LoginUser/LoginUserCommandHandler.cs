using ECommerenceAPI.Application.Abstractions.Services;
using ECommerenceAPI.Application.Abstractions.Token;
using ECommerenceAPI.Application.DTOs;
using ECommerenceAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly IAuthService _authService;

        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.LoginAsync(request.UserNameOrEmail,request.Password,15);
            return new LoginUserSuccessCommandResponse()
            {
                Token = token
            };
        }
    }
}

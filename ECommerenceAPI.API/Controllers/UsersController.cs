using ECommerenceAPI.Application.Abstractions.Services;
using ECommerenceAPI.Application.Features.Commands.AppUser.CreateUser;
using ECommerenceAPI.Application.Features.Commands.AppUser.FacebookLogin;
using ECommerenceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ECommerenceAPI.Application.Features.Commands.AppUser.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerenceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IMailService _mailService;

        public UsersController(IMediator mediator, IMailService mailService)
        {
            this._mediator = mediator;
            this._mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse createUserCommandResponse = await _mediator.Send(createUserCommandRequest);

            return Ok(createUserCommandResponse);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse loginUserCommandResponse = await _mediator.Send(loginUserCommandRequest);

            return Ok(loginUserCommandResponse);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse response = await _mediator.Send(googleLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin(FacebookLoginCommandRequest facebookLoginCommandRequest)
        {
            FacebookLoginCommandResponse response = await _mediator.Send(facebookLoginCommandRequest);
            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ExampleMailTest()
        {
            await _mailService.SendMessageAsync("ke_rem321@hotmail.com", "Siparişiniz tamamlandı aloooo", "Siparişiniz tamamlandı ve teslim edilmek üzere kargoya verildi kargo takip numaranız: kerbatgongotten3keremasallah");
            return Ok();
        }

    }
}

using System;
using Manager.API.Token;
using Manager.API.Utilities;
using Manager.API.ViewModes;
using Manager.Core.Communication.Messages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Manager.API.Controllers
{
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthController(
            IConfiguration configuration,
            ITokenService tokenService,
            INotificationHandler<DomainNotification> domainNotificationHandler)
            : base(domainNotificationHandler)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("/api/v1/auth/login")]
        public IActionResult Login([FromBody] LoginViewModel loginViewModel)
        {
            var tokenLogin = _configuration["Jwt:Login"];
            var tokenPassword = _configuration["Jwt:Password"];

            if (loginViewModel.Login == tokenLogin && loginViewModel.Password == tokenPassword)
                return Ok(new ResultViewModel
                {
                    Message = "Usuário autenticado com sucesso!",
                    Success = true,
                    Data = new
                    {
                        Token = _tokenService.GenerateToken(),
                        TokenExpires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:HoursToExpire"]))
                    }
                });
            else
                return StatusCode(401, Responses.UnauthorizedErrorMessage());
        }
    }
}
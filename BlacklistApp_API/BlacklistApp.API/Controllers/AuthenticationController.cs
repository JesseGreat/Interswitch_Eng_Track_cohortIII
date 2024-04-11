using BlacklistApp.Services.Interfaces;
using BlacklistApp.Services.Models;
using BlacklistApp.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlacklistApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {

        private readonly ILogger<UserController> _logger;
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(ILogger<UserController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("authenticate-user")]
        public async Task<IActionResult> CreateNewUserAsync(AuthenticateRequest authenticateRequest)
        {
            var response = await _authenticationService.AuthenticateAsync(authenticateRequest);
            return SendResponse(response);
        }
    }
}

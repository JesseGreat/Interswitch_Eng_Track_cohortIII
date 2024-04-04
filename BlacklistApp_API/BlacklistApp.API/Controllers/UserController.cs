using BlacklistApp.Services.Helpers;
using BlacklistApp.Services.Interfaces;
using BlacklistApp.Services.Models;
using BlacklistApp.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlacklistApp.API.Controllers
{
    [Authorization]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("create-new-user")]
        public async Task<IActionResult> CreateNewUserAsync(CreateUserRequest createUserRequest)
        {
            var response = await _userService.CreateUserAsync(createUserRequest);
            return SendResponse(response);
        }

        [HttpPost]
        [Route("update-user-details")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequest userRequest)
        {
            var response = await _userService.UpdateUserDetailsAsync(userRequest);
            return SendResponse(response);
        }

        [HttpPost]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUserAsync(UpdateUserRequest userRequest)
        {
            var response = await _userService.DeleteUserDetailsAsync(userRequest);
            return SendResponse(response);
        }

        [HttpPost]
        [Route("validate-user")]
        public async Task<IActionResult> ValidateUserAsync(ValidateUserRequest userRequest)
        {
            var response = await _userService.ValidateUserAsync(userRequest);
            return SendResponse(response);
        }

        [HttpGet]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var response = await _userService.GetAllUsersAsync();
            return SendResponse(response);
        }

        [HttpGet]
        [Route("get-all-roles")]
        public async Task<IActionResult> GetAllRolessAsync()
        {
            var response = _userService.GetAllUserRoles();
            return SendResponse(response);
        }

        [HttpGet]
        [Route("get-user/{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var response = await _userService.GetUserIdAsync(id);
            return SendResponse(response);
        }
    }
}

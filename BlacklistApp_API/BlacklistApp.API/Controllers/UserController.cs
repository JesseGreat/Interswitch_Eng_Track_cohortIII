using BlacklistApp.Services.Helpers;
using BlacklistApp.Services.Interfaces;
using BlacklistApp.Services.Models;
using BlacklistApp.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlacklistApp.API.Controllers
{
    [Authorization(UserRole.UserAdmin, UserRole.SuperUser)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("create-new-user")]
        public async Task<IActionResult> CreateNewUserAsync(CreateUserRequest createUserRequest)
        {

            var response = await _userService.CreateNewUserAsync(createUserRequest);
            return SendResponse(response);

        }

        [AllowAnonymous]

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
            var response = await _userService.DeleteUserAsync(userRequest);
            return SendResponse(response);
        }

        [AllowAnonymous]

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
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var response = await _userService.GetAllUserRolesAsync();
            return SendResponse(response);
        }

        [HttpGet]
        [Route("get-user/{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var response = await _userService.GetUserDetailsByIdAsync(id);
            return SendResponse(response);
        }
    }
}

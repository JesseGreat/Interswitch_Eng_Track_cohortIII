using BlacklistApp.Entities.Models;
using BlacklistApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result> CreateNewUserAsync(CreateUserRequest user);
        Task<Result> DeleteUserAsync(UpdateUserRequest user);
        Task<Result<List<object>>> GetAllUserRolesAsync();
        Task<Result<List<UserDetails>>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<Result<UserDetails>> GetUserDetailsByIdAsync(string id);
        Task<Result> UpdateUserDetailsAsync(UpdateUserRequest user);
        Task<Result<object>> ValidateUserAsync(ValidateUserRequest user);
    }
}

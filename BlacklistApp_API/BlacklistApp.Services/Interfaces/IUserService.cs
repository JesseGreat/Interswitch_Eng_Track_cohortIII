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
        Task<Result> CreateUserAsync(CreateUserRequest user);
        Task<Result> DeleteUserDetailsAsync(UpdateUserRequest user);
        Task<Result<List<UserDetails>>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task<Result<UserDetails>> GetUserIdAsync(string id);
        Task<Result> UpdateUserDetailsAsync(UpdateUserRequest user);
        Task<Result<object>> ValidateUserAsync(ValidateUserRequest user);
    }
}

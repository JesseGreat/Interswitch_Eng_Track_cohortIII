using BlacklistApp.Entities;
using BlacklistApp.Entities.Models;
using BlacklistApp.Services.Helpers;
using BlacklistApp.Services.Interfaces;
using BlacklistApp.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Services
{
    public class UserService : IUserService
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger<AuthenticationService> _logger;

        public UserService(IOptions<AppSettings> appSettings, RepositoryContext repositoryContext, ILogger<AuthenticationService> logger)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _repositoryContext = repositoryContext;
        }

        public async Task<Result<List<UserDetails>>> GetAllUsersAsync()
        {
            var users = await _repositoryContext.Users.Where(x => x.IsEnabled == true).ToListAsync();
            var response = new List<UserDetails>();
            foreach (var user in users)
            {
                response.Add(DataMapper.GetUser(user));
            }
            return new Result<List<UserDetails>>(true, "sucessful", response, StatusCodes.Status200OK);

        }

        public Result<List<object>> GetAllUserRoles()
        {

            return new Result<List<object>>(true, "Successful", Enum.GetValues(typeof(Models.UserRole))
               .Cast<Models.UserRole>()
               .Select(t => new
               {
                   Id = ((int)t),
                   Name = t.ToString()
               }).ToList<object>(), StatusCodes.Status200OK);
        }

        public async Task<Result<UserDetails>> GetUserIdAsync(string id)
        {
            var userDetails = await GetByIdAsync(id.Trim());
            if (userDetails == null || userDetails.IsEnabled == false)
                return new Result<UserDetails>(false, "User not found", new UserDetails(), StatusCodes.Status404NotFound);
            return new Result<UserDetails>(true, "sucessful", DataMapper.GetUser(userDetails), StatusCodes.Status200OK);

        }

        public async Task<User> GetByIdAsync(string id)
        {
            var valid = Guid.TryParse(id.Trim(), out Guid guid);
            if (!valid)
                return null;
            return await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Id == guid);

        }
        public async Task<Result> CreateUserAsync(CreateUserRequest user)
        {
            var allowedRoles = new List<int> { 1, 4 };
            var creator = await GetByIdAsync(user.CreatedBy.Trim());
            var newUser = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.EmailAdress.ToLower() == user.EmailAddress.Trim().ToLower());
            if (creator == null)
                return new Result(false, "This creating user does not exist", StatusCodes.Status401Unauthorized);
            if (!allowedRoles.Contains(creator.UserRoleId))
                return new Result(false, "The user does not have permission to perform this action.", StatusCodes.Status401Unauthorized);
            if (newUser != null)
                return new Result(false, $"User with email address {newUser.EmailAdress} already exists.", StatusCodes.Status400BadRequest);

            await _repositoryContext.Users.AddAsync(DataMapper.GetUser(user));
            await _repositoryContext.SaveChangesAsync();
            return new Result(true, "User created sucessfully.", 201);
        }
        public async Task<Result<object>> ValidateUserAsync(ValidateUserRequest user)
        {
            var userDetails = await _repositoryContext.Users.SingleOrDefaultAsync(x => x.EmailAdress.ToLower() == user.EmailAddress.Trim().ToLower());
            if (userDetails == null)
                return new Result(false, "Email Address does not exist", StatusCodes.Status400BadRequest);
            if (string.IsNullOrWhiteSpace(userDetails.Password))
                return new Result<object>(true, "Email validated successfully, Kindly proceed to update your details.", content: new { UserId = userDetails.Id.ToString() }, status: StatusCodes.Status200OK);
            else
                return new Result(true, "Please enter your password to proceed.", 200);
        }

        public async Task<Result> UpdateUserDetailsAsync(UpdateUserRequest user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
                return new Result(false, "Please provide a password to proceed.", StatusCodes.Status400BadRequest);

            var hashedPassword = Encryption.ComputeHash(user.Password.Trim(), string.Empty, null);

            var message = "Password changed successfully";

            var userDetails = await GetByIdAsync(user.Id.Trim());
            if (userDetails == null)
                return new Result(false, "User not found", StatusCodes.Status404NotFound);
            userDetails.Password = hashedPassword;
            if (!user.IsChangePassword)
            {
                userDetails.IsEnabled = true;
                userDetails.FullName = user.FullName.Trim();
                userDetails.DateLastModified = DateTime.Now;
                message = "User details updated successfully";
            }
            _repositoryContext.Update(userDetails);
            await _repositoryContext.SaveChangesAsync();
            return new Result(true, message, StatusCodes.Status200OK);

        }
        public async Task<Result> DeleteUserDetailsAsync(UpdateUserRequest user)
        {
            var creator = await GetByIdAsync(user.UpdatedBy.Trim());
            if (creator == null)
                return new Result(false, "This user does not exist", StatusCodes.Status401Unauthorized);
            if (creator.UserRoleId != 1)
                return new Result(false, "The user does not have permission to perform this action.", StatusCodes.Status401Unauthorized);

            var userDetails = await GetByIdAsync(user.Id.Trim());
            if (userDetails == null)
                return new Result(false, "User not found", StatusCodes.Status404NotFound);

            userDetails.UpdatedBy = user.UpdatedBy.Trim();
            userDetails.IsEnabled = false;
            userDetails.DateLastModified = DateTime.Now;
            _repositoryContext.Update(userDetails);
            await _repositoryContext.SaveChangesAsync();
            return new Result(true, "successful.", StatusCodes.Status200OK);

        }
    }
}

﻿using BlacklistApp.Entities;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;

        public UserService(RepositoryContext repositoryContext, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repositoryContext = repositoryContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<UserDetails>>> GetAllUsersAsync()
        {
            var users = await (from user in _repositoryContext.Users.Where(x => x.IsEnabled)
                               join creators in _repositoryContext.Users
                                     on user.CreatedBy.ToUpper() equals creators.Id.ToString().ToUpper()
                               select new UserDetails
                               {
                                   Id = user.Id.ToString(),
                                   CreatedBy = creators.FullName,
                                   EmailAdress = user.EmailAdress,
                                   DateCreated = user.DateCreated,
                                   FullName = user.FullName ?? string.Empty,
                                   UserRole = ((Models.UserRole)user.UserRoleId).ToString(),
                                   RoleId = user.UserRoleId
                               }).ToListAsync();

            return new Result<List<UserDetails>>(true, "sucessful", users, StatusCodes.Status200OK);

        }

        public async Task<Result<List<object>>> GetAllUserRolesAsync()
        {
            var userRoles = await _repositoryContext.UserRoles.Select(x => new { x.Id, x.Name }).ToListAsync<object>();
            return new Result<List<object>>(true, "Successful", userRoles, StatusCodes.Status200OK);
        }

        public async Task<Result<UserDetails>> GetUserDetailsByIdAsync(string id)
        {
            var valid = Guid.TryParse(id.Trim(), out Guid guid);
            if (!valid)
                return null;
            var userDetails = await (from user in _repositoryContext.Users.Where(x => x.IsEnabled == true && x.Id == guid)
                                     join creators in _repositoryContext.Users
                                     on user.CreatedBy.ToUpper() equals creators.Id.ToString().ToUpper()
                                     select new UserDetails
                                     {
                                         Id = user.Id.ToString(),
                                         CreatedBy = creators.FullName,
                                         EmailAdress = user.EmailAdress,
                                         DateCreated = user.DateCreated,
                                         FullName = user.FullName ?? string.Empty,
                                         UserRole = ((Models.UserRole)user.UserRoleId).ToString(),
                                         RoleId = user.UserRoleId
                                     }).SingleOrDefaultAsync();

            if (userDetails == null)
                return new Result<UserDetails>(false, "User not found", new UserDetails(), StatusCodes.Status404NotFound);
            return new Result<UserDetails>(true, "sucessful", userDetails, StatusCodes.Status200OK);

        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var valid = Guid.TryParse(id.Trim(), out Guid guid);
            if (!valid)
                return null;
            return await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Id == guid);

        }
        public async Task<Result> CreateNewUserAsync(CreateUserRequest user)
        {
            string userId = GetCurrentUserId();
            var newUser = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.EmailAdress.ToLower() == user.EmailAddress.Trim().ToLower());
            if (newUser != null)
                return new Result(false, $"User with email address {newUser.EmailAdress} already exists.", StatusCodes.Status400BadRequest);

            await _repositoryContext.Users.AddAsync(DataMapper.GetUser(user, userId));
            var response = await SaveChangesAsync();
            if(!response.Success)
                return new Result(false, $"{response.Content.Message}: {response.Content.InnerException?.Message}.", 500);
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
            string userId = GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(user.Password))
                return new Result(false, "Please provide a password to proceed.", StatusCodes.Status400BadRequest);

            var hashedPassword = Encryption.ComputeHash(user.Password.Trim(), string.Empty, null);

            var message = "Password changed successfully";

            var userDetails = await GetUserByIdAsync(user.Id.Trim());
            if (userDetails == null)
                return new Result(false, "User not found", StatusCodes.Status404NotFound);
            userDetails.Password = hashedPassword;
            if (!string.IsNullOrEmpty(userId))
            {
                userDetails.UpdatedBy = userId;
            }
            if (!user.IsChangePassword)
            {
                userDetails.IsEnabled = true;
                userDetails.FullName = user.FullName.Trim();
                userDetails.DateLastModified = DateTime.Now;
                message = "User details updated successfully";
            }
            _repositoryContext.Update(userDetails);
            var response = await SaveChangesAsync();
            if (!response.Success)
                return new Result(false, $"{response.Content.Message}: {response.Content.InnerException?.Message}.", 500);
            return new Result(true, message, StatusCodes.Status200OK);

        }
        public async Task<Result> DeleteUserAsync(UpdateUserRequest user)
        {
            string userId = GetCurrentUserId();


            var userDetails = await GetUserByIdAsync(user.Id.Trim());
            if (userDetails == null)
                return new Result(false, "User not found", StatusCodes.Status404NotFound);

            userDetails.UpdatedBy = userId.Trim();
            userDetails.IsEnabled = false;
            userDetails.DateLastModified = DateTime.Now;
            _repositoryContext.Update(userDetails);
            var response = await SaveChangesAsync();
            if (!response.Success)
                return new Result(false, $"{response.Content.Message}: {response.Content.InnerException?.Message}.", 500);
            return new Result(true, "successful.", StatusCodes.Status200OK);

        }
        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.Items["UserId"]?.ToString();
        }

        private async Task<Result<Exception>> SaveChangesAsync()
        {
            try
            {

                await _repositoryContext.SaveChangesAsync();
                return new Result<Exception>(true, content: null);

            }
            catch (Exception e)
            {

                return new Result<Exception>(false, e);
            }
        }
    }
}

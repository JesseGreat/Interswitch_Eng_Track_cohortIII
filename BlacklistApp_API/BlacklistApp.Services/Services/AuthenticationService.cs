using BlacklistApp.Entities;
using BlacklistApp.Entities.Models;
using BlacklistApp.Services.Helpers;
using BlacklistApp.Services.Interfaces;
using BlacklistApp.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(IOptions<AppSettings> appSettings, RepositoryContext repositoryContext, ILogger<AuthenticationService> logger)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _repositoryContext = repositoryContext;
        }
        public async Task<Result<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest model)
        {
            var hashedPassword = Encryption.ComputeHash(model.Password, string.Empty, null);

            var user = await (from users in _repositoryContext.Users
                                     join creators in _repositoryContext.Users
                                     on users.CreatedBy.ToLower() equals creators.Id.ToString().ToLower()
                                     select new User
                                     {
                                         Id = users.Id,
                                         FullName = users.FullName,
                                         CreatedBy = creators.FullName,
                                         UserRoleId = users.UserRoleId,
                                         DateCreated = users.DateCreated,
                                         IsEnabled = users.IsEnabled,
                                         DateLastModified = users.DateLastModified,
                                         UpdatedBy = users.UpdatedBy,
                                         EmailAdress = users.EmailAdress,
                                         Password = users.Password
                                     }).SingleOrDefaultAsync(x => x.IsEnabled == true && x.EmailAdress.ToLower() == model.Email.ToLower().Trim());

            if (user == null) return new Result<AuthenticateResponse>(false, "Authentication failed: User not found.", new AuthenticateResponse(), 404);

            var status = Encryption.VerifyHash(model.Password.Trim(), string.Empty, user.Password);
            if (!status) return new Result<AuthenticateResponse>(false, "Authentication failed: Password is incorrect.", new AuthenticateResponse(), 404);

            var token = GenerateToken(user);
            UserDetails userDetails = DataMapper.GetUser(user);
            return new Result<AuthenticateResponse>(true, "Authentication sucessful.", new AuthenticateResponse { Token = token, UserDetails = userDetails }, 200);

        }
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtKey));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>(){
                    new Claim("Id",Convert.ToString(user.Id)),
                    new Claim("FullName", user.FullName),
                    new Claim("Email", user.EmailAdress),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Role", Convert.ToString(user.UserRoleId))
            };

            var token = new JwtSecurityToken(_appSettings.JwtIssuer, _appSettings.JwtAudience, claims, expires: DateTime.UtcNow.AddMinutes(30), signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

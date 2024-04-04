using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using BlacklistApp.Services.Interfaces;

namespace BlacklistApp.Services.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate _next, IOptions<AppSettings> _appSettings)
        {
            this._next = _next;
            this._appSettings = _appSettings.Value;

        }
        public async Task Invoke(HttpContext context, IUserService userService)
        {

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
                //Validate Token
                AttachUserToContext(context, userService, token);
            _next(context);
        }

        private void AttachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtKey));
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _appSettings.JwtIssuer,
                    ValidAudience = _appSettings.JwtAudience
                }, out SecurityToken validateToken);


                var jwtToken = (JwtSecurityToken)validateToken;
                var userId = jwtToken.Claims.FirstOrDefault(_ => _.Type == "Id").Value;
                context.Items["User"] = userService.GetUserByIdAsync(userId);

            }
            catch 
            {
                throw;

            }
        }
    }
}

using BlacklistApp.Entities.Models;
using BlacklistApp.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static BlacklistApp.Services.Models.AuthorizationFilterModel;
using UserRole = BlacklistApp.Services.Models.UserRole;

namespace BlacklistApp.Services.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Authorization : Attribute, IAuthorizationFilter, IAllowAnonymousFilter
    {
        private readonly IList<UserRole> _roles;

        public Authorization(params UserRole[] _roles)
        {

            this._roles = _roles ?? new UserRole[] { UserRole.RegularUser };
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (HasAllowAnonymous(context))
            {
                // Allow anonymous access, no need to perform authorization check
                return;
            }
            var isRealUser = false;
            var item = context.HttpContext.Items["User"];
            if (item == null)
            {
                context.Result = new JsonResult(new Models.Result(false, "Unauthorized", StatusCodes.Status401Unauthorized));
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
                return;
                //return context.HttpContext.Response.WriteAsync();
            }
            var json = JsonConvert.SerializeObject(item);
            var result = JsonConvert.DeserializeObject<AuthorizationFilter>(json).Result;
            User user = JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(result));
            if (user == null)
            {
                context.Result = new JsonResult(new Models.Result(false, "Unauthorized", StatusCodes.Status401Unauthorized));
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
                return;
            }
            if (user != null)
                if (user.IsEnabled && _roles.Contains((UserRole)user.UserRoleId))
                {

                    // Set the user ID in the HttpContext
                    context.HttpContext.Items["UserId"] = user.Id;
                    isRealUser = true;
                }

            if (!isRealUser)
            {
                context.Result = new JsonResult(new Models.Result(false, "You don't have access to this module.", StatusCodes.Status403Forbidden));
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)StatusCodes.Status403Forbidden;
            }
        }

        private bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            // Check if any filter applied to the action or controller includes AllowAnonymousAttribute
            var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (!hasAllowAnonymous && context.ActionDescriptor.FilterDescriptors != null)
            {
                hasAllowAnonymous = context.ActionDescriptor.FilterDescriptors
                    .Any(filterDescriptor => filterDescriptor.Filter is IAllowAnonymous);
            }
            return hasAllowAnonymous;
        }
    }
}

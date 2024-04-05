using BlacklistApp.Entities.Models;
using BlacklistApp.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlacklistApp.Services.Models.AuthorizationFilterModel;

namespace BlacklistApp.Services.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Authorization : Attribute, IAuthorizationFilter
    {
        private readonly IList<int> _roles;

        public Authorization(params int[] _roles)
        {

            //this._roles = _roles ?? new int[] { 3 };
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var isRealUser = false;
            var item = context.HttpContext.Items["User"];
            var use = await Task.FromResult(item);
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
                if (user.IsEnabled)
                {
                    isRealUser = true;
                }

            if (!isRealUser)
            {
                context.Result = new JsonResult(new Models.Result(false, "Unauthorized", StatusCodes.Status401Unauthorized));
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
            }
        }
    }
}

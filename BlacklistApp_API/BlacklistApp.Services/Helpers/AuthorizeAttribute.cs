using BlacklistApp.Entities.Models;
using BlacklistApp.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

            this._roles = _roles ?? new int[] { 3 };
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isRolePermission = false;
            var item = context.HttpContext.Items["User"];
            var json = JsonConvert.SerializeObject(item);
            User user = JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<AuthorizationFilter>(json).Result));
            if (user == null)
            {
                context.Result = new JsonResult(new Models.Result(false, "Unauthorized", StatusCodes.Status401Unauthorized));
                //context.Result = new JsonResult(
                //        new { Message = "Unauthorization" }
                //    )
                //{ StatusCode = StatusCodes.Status401Unauthorized };


            }
            if (user != null && this._roles.Any())

                foreach (var AuthRole in this._roles)
                {

                    if (user.UserRoleId == AuthRole)
                    {
                        isRolePermission = true;

                    }

                }

            if (!isRolePermission)
                context.Result = new JsonResult(new Models.Result(false, "Unauthorized", StatusCodes.Status401Unauthorized));

            //context.Result = new JsonResult(
            //           new { Message = "Unauthorization" }
            //       )
            //{ StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}

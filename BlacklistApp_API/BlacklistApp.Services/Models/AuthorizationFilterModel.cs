using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Models
{
    public class AuthorizationFilterModel
    {
        public class Result
        {
            public string Id { get; set; }
            public string EmailAdress { get; set; }
            public string FullName { get; set; }
            public string Password { get; set; }
            public string CreatedBy { get; set; }
            public object UpdatedBy { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime DateLastModified { get; set; }
            public bool IsEnabled { get; set; }
            public int UserRoleId { get; set; }
            public object UserRole { get; set; }
        }

        public class AuthorizationFilter
        {
            public Result Result { get; set; }
            public int Id { get; set; }
            public object Exception { get; set; }
            public int Status { get; set; }
            public bool IsCanceled { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsCompletedSuccessfully { get; set; }
            public int CreationOptions { get; set; }
            public object AsyncState { get; set; }
            public bool IsFaulted { get; set; }
        }
    }
}

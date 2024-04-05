using BlacklistApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Models
{
    public class CreateUserRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public int UserRole { get; set; }
    }

    public class ValidateUserRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }

    public class UpdateUserRequest
    {

        [Required]
        public string Id { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Password { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsChangePassword { get; set; } = false;

    }

    public class UserDetails
    {
        public string Id { get; set; }

        public string EmailAdress { get; set; }

        public string FullName { get; set; }
        //public string Password { get; set; }
        //public string UserRole{ get; set; }
        public string CreatedBy { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        //public bool IsEnabled { get; set; }
        public string UserRole { get; set; }
        public int RoleId { get; set; }
    }

    public enum UserRole
    {
        UserAdmin = 1,
        BlacklistAdmin = 2,
        RegularUser = 3,
        SuperUser =4
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Entities.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string EmailAdress { get; set; }

        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated {get; set;}
        public DateTime? DateLastModified {get; set;}
        public bool IsEnabled { get; set; } = false;
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}

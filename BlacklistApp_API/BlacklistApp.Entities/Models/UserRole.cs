using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Entities.Models
{
    [Table("UserRole")]
    public class UserRole
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}

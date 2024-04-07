using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Entities.Models
{
    [Table("Items")]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        public bool IsBlacklisted { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public string CreatedBy { get; set; }
        //public User CreatedBy { get; set; }
        public List<BlacklistReason> BlacklistReasons { get; set; }

    }
}

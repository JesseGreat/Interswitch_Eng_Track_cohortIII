using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Entities.Models
{
    [Table("BlacklistReason")]
    public class BlacklistReason
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlacklist { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }

        public int ItemId { get; set; }
        public Item Items { get; set; }
        public string CreatedBy { get; set; }
    }
}

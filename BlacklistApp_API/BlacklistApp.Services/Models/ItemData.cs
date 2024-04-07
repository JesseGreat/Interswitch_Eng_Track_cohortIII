using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Models
{
    //request
    public class CreateItemRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string CreatedBy { get; set; } //Guid

    }



    public class BlacklistItemRequest
    {
        [Required]
        public string Reason { get; set; }
        [Required]
        public string UserId { get; set; } //guid
        public bool WillBlacklist { get; set; }
        [Required]
        public bool IsNewItem { get; set; }
        public string? ItemName { get; set; } //required for create and blacklist
        public int? ItemID { get; set; }

    }


    //response
    public class BlacklistItemItemDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        
        public DateTime DateCreated { get; set; }
        public DateTime DateBlacklisted { get; set; }
        public DateTime DateRestored { get; set; }
        public bool IsBlacklisted { get; set; }
        public string CreatedBy { get; set; } //actual Name
        public string? BlacklistedBy { get; set; }
        public string? RestoredBy { get; set; }

    }

}


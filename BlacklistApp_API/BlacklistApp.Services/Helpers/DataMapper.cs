using BlacklistApp.Entities.Models;
using BlacklistApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Helpers
{
    public class DataMapper
    {
        public static User GetUser(CreateUserRequest userRequest) => new()
        {
            CreatedBy = userRequest.CreatedBy,
            UserRoleId = userRequest.UserRole,
            EmailAdress = userRequest.EmailAddress,
            DateCreated = DateTime.Now
        };

        public static UserDetails GetUser(User user) => new()
        {
            Id = user.Id.ToString(),
            CreatedBy = user.CreatedBy,
            EmailAdress = user.EmailAdress,
            DateCreated = user.DateCreated,
            FullName = user.FullName ?? string.Empty,
            UserRole = ((Models.UserRole)user.UserRoleId).ToString(),
            RoleId = user.UserRoleId
        };

        public static Item CreateItem(string itemName, string userId, bool willBlackList) => new()
        {
            DateCreated = DateTime.Now,
            Name = itemName,
            CreatedBy = userId,
            IsBlacklisted = willBlackList
        };

        public static BlacklistReason BlacklistItem(BlacklistItemRequest itemRequest) => new()
        {
            Reason = itemRequest.Reason,
            CreatedBy = itemRequest.UserId,
            IsActive = true,
            IsBlacklist = itemRequest.WillBlacklist,
            DateCreated = DateTime.Now,
            ItemId = itemRequest.ItemID.Value

        };
    }
}

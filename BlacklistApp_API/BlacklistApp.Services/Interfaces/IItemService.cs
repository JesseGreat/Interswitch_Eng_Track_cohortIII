using BlacklistApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Interfaces
{
    public interface IItemService
    {
        Task<Result> BlacklistItemAsync(BlacklistItemRequest blacklistItemRequest);
        Task<Result<List<object>>> CheckIfItemsAreBlacklistedAsync(List<int> items);
        Task<Result> CreateItemAsync(CreateItemRequest createItemRequest);
        Task<Result> CreateItemAndBlacklistAsync(BlacklistItemRequest blacklistItemRequest);
        Task<Result<List<object>>> GetAllItemsAsync();
        Task<Result<List<object>>> GetAllItemsByStatusAsync(bool isBlacklisted);
        Task<Result<BlacklistItemItemDetails>> GetBlacklistedItemByIdAsync(int id);
        Task<Result<BlacklistItemItemDetails>> GetWhitelistedItemByIdAsync(int id);
        Task<Result<List<object>>> GetItemHistoryByIdAsync(int id);
    }
}

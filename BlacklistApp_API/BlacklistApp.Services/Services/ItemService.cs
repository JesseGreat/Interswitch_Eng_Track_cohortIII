using BlacklistApp.Entities;
using BlacklistApp.Entities.Models;
using BlacklistApp.Services.Helpers;
using BlacklistApp.Services.Interfaces;
using BlacklistApp.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Services
{
    public class ItemService : IItemService
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ItemService> _logger;

        public ItemService(RepositoryContext repositoryContext, ILogger<ItemService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repositoryContext = repositoryContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result> CreateItemAndBlacklistAsync(BlacklistItemRequest blacklistItemRequest)
        {
            string userId = GetCurrentUserId();

            if (blacklistItemRequest is null)
                return new Result(false, "Invalid request.", StatusCodes.Status400BadRequest);

            if (string.IsNullOrWhiteSpace(blacklistItemRequest.ItemName) && blacklistItemRequest.IsNewItem)
                return new Result(false, "Item name cannot be null.", StatusCodes.Status400BadRequest);

            var itemRequest = DataMapper.CreateItem(blacklistItemRequest.ItemName, userId, true);
            _repositoryContext.Items.Add(itemRequest);
            var response = await SaveChangesAsync();
            if (!response.Success)
                return new Result(false, $"{response.Content.Message}: {response.Content.InnerException?.Message}.", 500);
            blacklistItemRequest.ItemID = itemRequest.Id;
            _repositoryContext.BlacklistReasons.Add(DataMapper.BlacklistItem(blacklistItemRequest, userId));
            var response1 = await SaveChangesAsync();
            if (!response1.Success)
                return new Result(false, $"{response1.Content.Message}: {response1.Content.InnerException?.Message}.", 500);

            return new Result(true, "Item blacklisted successfully.", StatusCodes.Status201Created);
        }
        public async Task<Result> BlacklistItemAsync(BlacklistItemRequest blacklistItemRequest)
        {
            string userId = GetCurrentUserId();

            if (blacklistItemRequest is null)
                return new Result(false, "Invalid request.", StatusCodes.Status400BadRequest);

            if (blacklistItemRequest.ItemID == 0 || blacklistItemRequest.ItemID is null)
                return new Result(false, "Item id cannot be null.", StatusCodes.Status400BadRequest);

            var itemHistory = _repositoryContext.BlacklistReasons.Where(x => x.ItemId == blacklistItemRequest.ItemID && x.IsBlacklist != blacklistItemRequest.WillBlacklist && x.IsActive).FirstOrDefault();
            if (itemHistory != null)
            {
                itemHistory.IsActive = false;
                _repositoryContext.BlacklistReasons.Update(itemHistory);
            }

            var item = await _repositoryContext.Items.Where(x => x.Id == blacklistItemRequest.ItemID && x.IsBlacklisted != blacklistItemRequest.WillBlacklist).SingleOrDefaultAsync();
            if (item != null)
            {
                item.IsBlacklisted = blacklistItemRequest.WillBlacklist;
                _repositoryContext.Items.Update(item);
            }
            _repositoryContext.BlacklistReasons.Add(DataMapper.BlacklistItem(blacklistItemRequest, userId));
            var response = await SaveChangesAsync();
            if (!response.Success)
                return new Result(false, $"{response.Content.Message}: {response.Content.InnerException?.Message}.", 500);

            return new Result(true, $"Item {(blacklistItemRequest.WillBlacklist ? "added to" : "removed from")} blacklist successfully.", StatusCodes.Status200OK);
        }

        public async Task<Result> CreateItemAsync(CreateItemRequest createItemRequest)
        {
            string userId = GetCurrentUserId();

            if (createItemRequest is null)
                return new Result(false, "Invalid request.", StatusCodes.Status400BadRequest);

            if (string.IsNullOrWhiteSpace(createItemRequest.Name))
                return new Result(false, "Item name cannot be null.", StatusCodes.Status400BadRequest);

            var itemRequest = DataMapper.CreateItem(createItemRequest.Name, userId, false);
            _repositoryContext.Items.Add(itemRequest);

            var response = await SaveChangesAsync();
            if (!response.Success)
                return new Result(false, $"{response.Content.Message}: {response.Content.InnerException?.Message}.", 500);

            return new Result(true, "Item created successfully.", StatusCodes.Status201Created);
        }
        public async Task<Result<BlacklistItemItemDetails>> GetBlacklistedItemByIdAsync(int id)
        {

            if (id == 0)
                return new Result<BlacklistItemItemDetails>(false, "Item id cannot be null.", StatusCodes.Status400BadRequest);
            var item = await (from items in _repositoryContext.Items.Where(x => x.Id == id && x.IsBlacklisted)
                              join creators in _repositoryContext.Users
                              on items.CreatedBy.ToUpper() equals creators.Id.ToString().ToUpper()
                              join actions in _repositoryContext.BlacklistReasons.Where(x => x.IsBlacklist && x.IsActive)
                              on items.Id equals actions.ItemId
                              join blacklisters in _repositoryContext.Users
                              on actions.CreatedBy.ToUpper() equals blacklisters.Id.ToString().ToUpper()
                              select new BlacklistItemItemDetails
                              {
                                  Id = items.Id,
                                  Name = items.Name,
                                  Reason = actions.Reason,
                                  DateCreated = items.DateCreated,
                                  DateBlacklisted = actions.DateCreated,
                                  CreatedBy = creators.FullName,
                                  BlacklistedBy = blacklisters.FullName,
                                  IsBlacklisted = items.IsBlacklisted
                              }).FirstOrDefaultAsync();
            if (item is null)
                return new Result<BlacklistItemItemDetails>(false, "Item details could not be retrieved. Please ensure it is a blacklisted item.", StatusCodes.Status404NotFound);

            return new Result<BlacklistItemItemDetails>(true, "Item details retrieved successfully.", item, StatusCodes.Status200OK);
        }
        public async Task<Result<BlacklistItemItemDetails>> GetWhitelistedItemByIdAsync(int id)
        {

            if (id == 0)
                return new Result<BlacklistItemItemDetails>(false, "Item id cannot be null.", StatusCodes.Status400BadRequest);
            var item = await (from items in _repositoryContext.Items.Where(x => x.Id == id && !x.IsBlacklisted)
                              join creators in _repositoryContext.Users
                              on items.CreatedBy.ToUpper() equals creators.Id.ToString().ToUpper()
                              join blacklist in _repositoryContext.BlacklistReasons.Where(x => x.IsBlacklist).OrderByDescending(x => x.Id)
                              on items.Id equals blacklist.ItemId
                              join blacklisters in _repositoryContext.Users
                              on blacklist.CreatedBy.ToUpper() equals blacklisters.Id.ToString().ToUpper()
                              join whitelist in _repositoryContext.BlacklistReasons.Where(x => !x.IsBlacklist && x.IsActive)
                              on items.Id equals whitelist.ItemId
                              join whitelister in _repositoryContext.Users
                              on whitelist.CreatedBy.ToUpper() equals whitelister.Id.ToString().ToUpper()
                              select new BlacklistItemItemDetails
                              {
                                  Id = items.Id,
                                  Name = items.Name,
                                  Reason = whitelist.Reason,
                                  DateCreated = items.DateCreated,
                                  DateBlacklisted = blacklist.DateCreated,
                                  DateRestored = whitelist.DateCreated,
                                  CreatedBy = creators.FullName,
                                  BlacklistedBy = blacklisters.FullName,
                                  RestoredBy = whitelister.FullName,
                                  IsBlacklisted = items.IsBlacklisted
                              }).FirstOrDefaultAsync();
            if (item is null)
                return new Result<BlacklistItemItemDetails>(false, "Item details could not be retrieved. Please ensure it is not a blacklisted item.", StatusCodes.Status404NotFound);

            return new Result<BlacklistItemItemDetails>(true, "Item details retrieved successfully.", item, StatusCodes.Status200OK);
        }
        public async Task<Result<List<object>>> GetItemHistoryByIdAsync(int id)
        {

            if (id == 0)
                return new Result<List<object>>(false, "Item id cannot be null.", StatusCodes.Status400BadRequest);
            var item = await (from items in _repositoryContext.Items.Where(x => x.Id == id)
                              join creators in _repositoryContext.Users
                              on items.CreatedBy.ToUpper() equals creators.Id.ToString().ToUpper()
                              join actions in _repositoryContext.BlacklistReasons
                              on items.Id equals actions.ItemId
                              join users in _repositoryContext.Users
                              on actions.CreatedBy.ToUpper() equals users.Id.ToString().ToUpper()
                              select new 
                              {
                                  Name = items.Name,
                                  ActionReason = actions.Reason,
                                  ItemCreatedBy = creators.FullName,
                                  DateItemCreated = items.DateCreated,
                                  DateOfAction = actions.DateCreated,
                                  ActionPerformedBy = users.FullName,
                                  IsCurrentAction = actions.IsActive,
                                  IsBlacklist = actions.IsBlacklist
                              }).ToListAsync<object>();

            return new Result<List<object>>(true, "Item details retrieved successfully.", item, StatusCodes.Status200OK);
        }
        public async Task<Result<List<object>>> GetAllItemsByStatusAsync(bool isBlacklisted)
        {
            var item = await _repositoryContext.Items.Where(x => x.IsBlacklisted == isBlacklisted).Select(x => new { x.Id, x.Name, x.DateCreated }).ToListAsync<object>();
            return new Result<List<object>>(true, "Item details retrieved successfully.", item, StatusCodes.Status200OK);
        }
        public async Task<Result<List<object>>> GetAllItemsAsync()
        {
            var item = await _repositoryContext.Items.Select(x => new { x.Id, x.Name, x.IsBlacklisted }).ToListAsync<object>();
            return new Result<List<object>>(true, "Item details retrieved successfully.", item, StatusCodes.Status200OK);
        }
        public async Task<Result<List<object>>> CheckIfItemsAreBlacklistedAsync(List<int> items)
        {
            var item = await _repositoryContext.Items.Where(x => items.Contains(x.Id)).Select(x => new { x.Id, x.Name, x.IsBlacklisted }).ToListAsync<object>();
            return new Result<List<object>>(true, "Item details retrieved successfully.", item, StatusCodes.Status200OK);
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.Items["UserId"]?.ToString();
        }
        private async Task<Result<Exception>> SaveChangesAsync()
        {
            try
            {

                await _repositoryContext.SaveChangesAsync();
                return new Result<Exception>(true, content: null);

            }
            catch (Exception e)
            {

                return new Result<Exception>(false, e);
            }
        }
    }
}

using BlacklistApp.Services.Helpers;
using BlacklistApp.Services.Interfaces;
using BlacklistApp.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlacklistApp.API.Controllers
{
    [Authorization(UserRole.BlacklistAdmin, UserRole.SuperUser)]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("check-if-items-are-blacklisted")]
        public async Task<IActionResult> CreateNewUserAsync(List<int> itemIds)
        {
            var response = await _itemService.CheckIfItemsAreBlacklistedAsync(itemIds);
            return SendResponse(response);
        }

        [HttpPost]
        [Route("create-and-blacklist")]
        public async Task<IActionResult> CreateItemAndBlacklistAsync(BlacklistItemRequest blacklistItemRequest)
        {
            var response = await _itemService.CreateItemAndBlacklistAsync(blacklistItemRequest);
            return SendResponse(response);
        }

        [HttpPost]
        [Route("add-blacklist")]
        public async Task<IActionResult> AddBlacklistAsync(BlacklistItemRequest blacklistItemRequest)
        {
            var response = await _itemService.BlacklistItemAsync(blacklistItemRequest);
            return SendResponse(response);
        }

        [HttpPost]
        [Route("remove-blacklist")]
        public async Task<IActionResult> RemoveBlacklistAsync(BlacklistItemRequest blacklistItemRequest)
        {
            var response = await _itemService.BlacklistItemAsync(blacklistItemRequest);
            return SendResponse(response);
        }

        [HttpPost]
        [Route("create-item")]
        public async Task<IActionResult> CreateAsync(CreateItemRequest createItemRequest)
        {
            var response = await _itemService.CreateItemAsync(createItemRequest);
            return SendResponse(response);
        }

        [HttpGet]
        [Route("get-blacklist-detail/{itemId}")]
        public async Task<IActionResult> GetBlacklistedItemByIdAsync(int itemId)
        {
            var response = await _itemService.GetBlacklistedItemByIdAsync(itemId);
            return SendResponse(response);
        }
        [HttpGet]
        [Route("get-whitelist-detail/{itemId}")]
        public async Task<IActionResult> GetWhitelistedItemByIdAsync(int itemId)
        {
            var response = await _itemService.GetWhitelistedItemByIdAsync(itemId);
            return SendResponse(response);
        }
        [HttpGet]
        [Route("get-item-history/{itemId}")]
        public async Task<IActionResult> GetItemHistoryByIdAsync(int itemId)
        {
            var response = await _itemService.GetItemHistoryByIdAsync(itemId);
            return SendResponse(response);
        }

        [HttpGet]
        [Route("get-blacklist-items")]
        public async Task<IActionResult> GetAllBlacklistedItemsAsync()
        {
            var response = await _itemService.GetAllItemsByStatusAsync(true);
            return SendResponse(response);
        }

        [HttpGet]
        [Route("get-whitelist-items")]
        public async Task<IActionResult> GetAllAllowedItemsAsync()
        {
            var response = await _itemService.GetAllItemsByStatusAsync(false);
            return SendResponse(response);
        }

        [HttpGet]
        [Route("get-all-items")]
        public async Task<IActionResult> GetAllItemsAsync()
        {
            var response = await _itemService.GetAllItemsAsync();
            return SendResponse(response);
        }


    }
}

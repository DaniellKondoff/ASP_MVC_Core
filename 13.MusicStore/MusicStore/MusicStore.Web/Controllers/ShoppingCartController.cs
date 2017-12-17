using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Data.Models;
using MusicStore.Services.Contracts;
using MusicStore.Services.Models.Songs;
using MusicStore.Web.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartManager shoppingCartManager;
        private readonly ISongService songService;
        private readonly UserManager<User> userManager;
        private readonly IShoppingService shoppingService;

        public ShoppingCartController(
            IShoppingCartManager shoppingCartManager, 
            ISongService songService, 
            UserManager<User> userManager,
            IShoppingService shoppingService)
        {
            this.shoppingCartManager = shoppingCartManager;
            this.songService = songService;
            this.userManager = userManager;
            this.shoppingService = shoppingService;
        }

        public async Task<IActionResult> Items()
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();

            var itemsWithDetails = await this.GetCartItemsWithDetails(shoppingCartId);

            return View(itemsWithDetails);
        }

        public IActionResult AddToCart(int id)
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();

            this.shoppingCartManager.AddToCart(shoppingCartId, id);

            return RedirectToAction(nameof(Items));
        }

        [Authorize]
        public async Task<IActionResult> FinishOrder()
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();

            var itemsWithDetails = await this.GetCartItemsWithDetails(shoppingCartId);
            var userId = this.userManager.GetUserId(User);

            await this.shoppingService.CreateOrderAsync(userId, itemsWithDetails);

            this.shoppingCartManager.Clear(shoppingCartId);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var shoppingCartId = this.HttpContext.Session.GetShoppingCartId();

            this.shoppingCartManager.RemoveFromCart(shoppingCartId, id);

            return RedirectToAction(nameof(Items));
        }

        private async Task<IEnumerable<SongShoppingDetailsServiceModel>> GetCartItemsWithDetails(string shoppingCartId)
        {
            var items = this.shoppingCartManager.GetItems(shoppingCartId);
            var itemIds = items.Select(i => i.SongId);

            var itemQuantities = items.ToDictionary(i => i.SongId, i => i.Quantity);


            var itemsWithDetails = await this.songService.SongShoppingDetails(itemIds);

            foreach (var item in itemsWithDetails)
            {
                item.Quantity = itemQuantities[item.Id];
            }

            return itemsWithDetails;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Services.Contracts;
using MusicStore.Web.Infrastructure.ShoppingCartIService;
using MusicStore.Web.Models.ShoppingCartViewModels;
using System.Threading.Tasks;

namespace MusicStore.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCart shoppingCart;
        private readonly ISongService songService;

        public ShoppingCartController(ShoppingCart shoppingCart, ISongService songService)
        {
            this.shoppingCart = shoppingCart;
            this.songService = songService;
        }

        public IActionResult Index()
        {
            var items = shoppingCart.GetShoppingCartItems();
            shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }

        public async Task<IActionResult> AddToShoppingCart(int Id)
        {
            var song = await this.songService.DetailsAsync(Id);

            if (song != null)
            {
                shoppingCart.AddToCart(song.Id, 1);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

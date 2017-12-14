using Microsoft.AspNetCore.Mvc;
using MusicStore.Web.Infrastructure.ShoppingCartIService;
using MusicStore.Web.Models.ShoppingCartViewModels;

namespace MusicStore.Web.Components
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCart shoppingCart;

        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            this.shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            var items = shoppingCart.GetShoppingCartItems();
            //var items = new List<ShoppingCartItem>() { new ShoppingCartItem(), new ShoppingCartItem() };
            shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }
    }
}

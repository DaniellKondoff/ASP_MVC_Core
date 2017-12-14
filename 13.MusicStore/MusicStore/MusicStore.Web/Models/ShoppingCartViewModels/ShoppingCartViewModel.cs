using MusicStore.Web.Infrastructure.ShoppingCartIService;

namespace MusicStore.Web.Models.ShoppingCartViewModels
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }
        public decimal ShoppingCartTotal { get; set; }
    }
}

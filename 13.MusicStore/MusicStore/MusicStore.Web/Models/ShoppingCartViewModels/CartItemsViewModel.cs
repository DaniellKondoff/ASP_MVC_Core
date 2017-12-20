using MusicStore.Services.Models.Albums;
using MusicStore.Services.Models.Songs;
using System.Collections.Generic;

namespace MusicStore.Web.Models.ShoppingCartViewModels
{
    public class CartItemsViewModel
    {
        public IEnumerable<SongShoppingDetailsServiceModel> ShoppingSongs { get; set; }

        public IEnumerable<AlbumShoppingDetailsServiceModels> ShoppingAlbums { get; set; }
    }
}

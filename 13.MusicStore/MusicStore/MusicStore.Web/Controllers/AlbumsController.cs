using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Services.Contracts;
using MusicStore.Web.Models.AlbumsViewModels;
using System.Threading.Tasks;

namespace MusicStore.Web.Controllers
{
    [Authorize]
    public class AlbumsController : Controller
    {
        private readonly IAlbumService albumService;

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> ListAll(int page = 1)
        {
            var albums = await this.albumService.ListAllAsync(page);

            return View(new AlbumListingViewModel
            {
                AllAlbums = albums,
                CurrentPage = page,
                TotalAlbums = await this.albumService.TotalAsync()
            });
        }

        public async Task<IActionResult> Details(int Id)
        {
            var album = await this.albumService.DetailsAsync(Id);

            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }
    }
}

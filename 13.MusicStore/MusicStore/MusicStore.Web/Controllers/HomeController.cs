using Microsoft.AspNetCore.Mvc;
using MusicStore.Services.Contracts;
using MusicStore.Web.Models;
using MusicStore.Web.Models.HomeViewModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MusicStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArtistService artistService;
        private readonly ISongService songService;
        private readonly IAlbumService albumService;

        public HomeController(IArtistService artistService, ISongService songService, IAlbumService albumService)
        {
            this.artistService = artistService;
            this.songService = songService;
            this.albumService = albumService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(SearchFormModel model)
        {
            var viewModel = new SearchViewModel
            {
                SearchText = model.SearchText
            };

            if (model.SearchInArtists)
            {
                viewModel.Artists = await this.artistService.FindAsync(model.SearchText);
            }

            if (model.SearchInSongs)
            {
                viewModel.Songs = await this.songService.FindAsync(model.SearchText);
            }

            if (model.SearchInAlbums)
            {
                viewModel.Albums = await this.albumService.FindAsync(model.SearchText);
            }

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

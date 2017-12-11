using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Web.Areas.Admin.Models.Albums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Web.Areas.Admin.Controllers
{
    public class AlbumsController : BaseAdminController
    {
        private readonly IAdminArtistService artistService;
        private readonly IAdminAlbumService albumService;

        public AlbumsController(IAdminArtistService artistService, IAdminAlbumService albumService)
        {
            this.artistService = artistService;
            this.albumService = albumService;
        }

        public async Task<IActionResult> Add()
        {
            return View(new AlbumFormViewModel
            {
                Artists = await this.GetArtists()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AlbumFormViewModel model)
        {
            var isArtistExisting = await this.artistService.ExistAsync(model.ArtistId);

            if (!isArtistExisting)
            {
                ModelState.AddModelError(nameof(model.ArtistId), "Please select valid Artist");
            }

            if (!ModelState.IsValid)
            {
                return View(new AlbumFormViewModel
                {
                    Artists = await this.GetArtists()
                });
            }

            await this.albumService.CreateAsync(model.Title, model.Price, model.AmountOfSongs, model.ArtistId);

            return RedirectToAction(nameof(ListAll));
        }

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

        public async Task<IActionResult> Edit(int id)
        {
            var album = await this.albumService.GetByIdAsync(id);

            if (album == null)
            {
                return NotFound();
            }


            return View(new AlbumFormViewModel
            {
                Title = album.Title,
                Price = album.Price,
                AmountOfSongs = album.AmountOfSong,
                Artists = await this.GetArtists()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AlbumFormViewModel model)
        {
            var IsExisting = await this.albumService.ExistAsync(id);

            if (!IsExisting)
            {
                ModelState.AddModelError(nameof(id), "There is no such albums");
            }

            if (!ModelState.IsValid)
            {
                return View(new AlbumFormViewModel
                {
                    Title = model.Title,
                    Price = model.Price,
                    AmountOfSongs = model.AmountOfSongs,
                    Artists = await this.GetArtists()
                });
            }

            await this.albumService.EditAsync(id, model.Title, model.Price, model.AmountOfSongs, model.ArtistId);

            return RedirectToAction(nameof(ListAll));
        }


        private async Task<IEnumerable<SelectListItem>> GetArtists()
        {
            var artists = await this.artistService.AllBasicAsync();

            var artistListItems = artists
                .Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                })
                .ToList();

            return artistListItems;
        }
    }
}

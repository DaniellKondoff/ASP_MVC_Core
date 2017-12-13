using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Web.Areas.Admin.Models.Albums;
using MusicStore.Web.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Web.Areas.Admin.Controllers
{
    public class AlbumsController : BaseAdminController
    {
        private readonly IAdminArtistService artistService;
        private readonly IAdminAlbumService albumService;
        private readonly IAdminSongService songService;

        public AlbumsController(IAdminArtistService artistService, IAdminAlbumService albumService, IAdminSongService songService)
        {
            this.artistService = artistService;
            this.albumService = albumService;
            this.songService = songService;
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

        //TODO Amount of Song
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

        public IActionResult Delete(int Id)
        {
            return View(Id);
        }

        public async Task<IActionResult> Destroy(int id)
        {
            var success = await this.albumService.DeleteAsync(id);

            if (!success)
            {
                TempData.AddErrorMessage("Invalid Request");
            }
            else
            {
                TempData.AddSuccessMessage("Album has been deleted successfully");
            }
            return RedirectToAction(nameof(ListAll));
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

        public async Task<IActionResult> AddSongTo (int Id)
        {
            var album = await this.albumService.GetByIdAsync(Id);

            if (album == null)
            {
                return NotFound();
            }

            var artistId = await this.albumService.GetArtistIdByAlbumId(Id);

            return View(new AlbumAddingSongViewModel
            {
                Id = Id,
                Title = album.Title,
                Songs = await this.GetSongs(artistId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddSongTo(AlbumAddingSongFormViewModel model)
        {
            var album = await this.albumService.GetByIdAsync(model.Id);

            if (album == null)
            {
                ModelState.AddModelError(nameof(model.Id), "Invalid Album");
            }

            var artistId = await this.albumService.GetArtistIdByAlbumId(model.Id);

            bool IsSongAlreadyAdded = await this.albumService.IsSongAddedAsync(model.Id, model.SongId);

            if (IsSongAlreadyAdded)
            {
                ModelState.AddModelError(nameof(model.Id), "That Song already has been added");
            }

            if (!ModelState.IsValid)
            {
                return View(new AlbumAddingSongViewModel
                {
                    Id = model.Id,
                    Title = album.Title,
                    Songs = await this.GetSongs(artistId)
                });
            }


            bool success = await this.albumService.AddSongToAlbumAsync(model.Id, model.SongId, artistId);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("You have successfuly add song to Album");

            return RedirectToAction(nameof(Details), new { Id = model.Id });
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

        private async Task<IEnumerable<SelectListItem>> GetSongs(int Id)
        {
            var songs = await this.songService.AllBasicAsync(Id);

            var songsListItems = songs
                .Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                })
                .ToList();

            return songsListItems;
        }
    }
}

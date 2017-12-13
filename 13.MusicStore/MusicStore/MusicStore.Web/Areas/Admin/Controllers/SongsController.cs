using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Web.Areas.Admin.Models.Songs;
using MusicStore.Web.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Web.Areas.Admin.Controllers
{
    public class SongsController : BaseAdminController
    {
        private readonly IAdminArtistService artistService;
        private readonly IAdminSongService songService;

        public SongsController(IAdminArtistService artistService, IAdminSongService songService)
        {
            this.artistService = artistService;
            this.songService = songService;
        }

        public async Task<IActionResult> Add()
        {
            return View(new SongFormViewModel
            {
               Artists =await this.GetArtists()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(SongFormViewModel model)
        {
            if (model.Ganre < 0)
            {
                ModelState.AddModelError(nameof(model.Ganre), "Please select at least one Ganre");
            }

            var isArtistExisting = await this.artistService.ExistAsync(model.ArtistId);

            if (!isArtistExisting)
            {
                ModelState.AddModelError(nameof(model.ArtistId), "Please select valid Artist");
            }

            if (!ModelState.IsValid)
            {
                return View(new SongFormViewModel
                {
                    Artists = await this.GetArtists()
                });
            }

            await this.songService.CreateAsync(model.Name, model.Price, model.Duration, model.ArtistId, model.Ganre);

            return RedirectToAction(nameof(ListAll));
        }

        public async Task<IActionResult> ListAll(int page = 1)
        {
            var allSongs = await this.songService.AllAsync(page);

            return View(new SongListingViewModel
            {
                AllSongs = allSongs,
                CurrentPage = page,
                TotalSongs = await this.songService.TotalAsync()
            });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var song = await this.songService.GetByIdAsync(id);

            if (song == null)
            {
                return BadRequest();
            }

            return View(new SongFormViewModel
            {
                Name = song.Name,
                Price = song.Price,
                Duration = song.Duration,
                Artists = await this.GetArtists(song.Artist)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SongFormViewModel model)
        {
            var IsSongExist = await this.songService.ExistAsync(id);

            if (!IsSongExist)
            {
                ModelState.AddModelError(nameof(id), "Invalid Song");
            }

            if (model.Ganre < 0)
            {
                ModelState.AddModelError(nameof(model.Ganre), "Please select at least one Ganre");
            }

            if (!ModelState.IsValid)
            {
                return View(new SongFormViewModel
                {
                    Name = model.Name,
                    Price = model.Price,
                    Duration = model.Duration,
                    Artists = await this.GetArtists()
                });
            }

            await this.songService.EditAsync(id,model.Name, model.Price, model.Duration, model.ArtistId, model.Ganre);

            TempData.AddSuccessMessage($" Song {model.Name} has been edited successfully");
            return RedirectToAction(nameof(ListAll));
        }

        public IActionResult Delete(int id)
        {
            return View(id);
        }

        public async Task<IActionResult> Destroy(int id)
        {
            var success = await this.songService.DeleteAsync(id);

            if (!success)
            {
                TempData.AddErrorMessage("Invalid Request");
            }
            else
            {
                TempData.AddSuccessMessage("Song has been deleted successfully");
            }
            return RedirectToAction(nameof(ListAll));

        }

        public async Task<IActionResult> Details(int id)
        {
            var song = await this.songService.DetailsAsync(id);

            if (song == null)
            {
                return BadRequest();
            }

            return View(song);
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

        private async Task<IEnumerable<SelectListItem>> GetArtists(string artistName)
        {
            var artists = await this.artistService.AllBasicAsync();

            var artistListItems = artists
                .Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                    Selected = artistName.Contains(a.Name)
                })
                .ToList();

            return artistListItems;
        }
    }
}

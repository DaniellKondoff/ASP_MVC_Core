using Microsoft.AspNetCore.Mvc;
using MusicStore.Data.Enums;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Web.Areas.Admin.Models.Artists;
using MusicStore.Web.Infrastructure.Extensions;
using MusicStore.Web.Infrastructure.Filters;
using System.Threading.Tasks;
using static MusicStore.Web.Infrastructure.Common.WebConstants;

namespace MusicStore.Web.Areas.Admin.Controllers
{
    public class ArtistsController : BaseAdminController
    {
        private readonly IAdminArtistService artistService;

        public ArtistsController(IAdminArtistService artistService)
        {
            this.artistService = artistService;
        }

        public async Task<IActionResult> ListAll(int page = 1)
        {
            var allArtist = await this.artistService.AllAsync(page);

            return View(new ArtistListingViewModel
            {
                AllArtists = allArtist,
                TotalArtists = await this.artistService.TotalAsync(),
                CurrentPage = page
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        [Log(Operation.Add, ArtistTable)]
        public async Task<IActionResult> Create(ArtistFormViewModel model)
        {
            await this.artistService.CreateAsync(model.Name);

            return RedirectToAction(nameof(ListAll));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var artist = await this.artistService.GetByIdAsync(id);

            if (artist == null)
            {
                return NotFound();
            }

            return View(new ArtistFormViewModel
            {
                Name = artist.Name
            });

        }

        [HttpPost]
        [ValidateModelState]
        [Log(Operation.Edit, ArtistTable)]
        public async Task<IActionResult> Edit(int id, ArtistFormViewModel model)
        {
            var success = await this.artistService.EditAsync(id, model.Name);

            if (!success)
            {
                TempData.AddErrorMessage("Invalid Request");
            }
            else
            {
                TempData.AddSuccessMessage($" Artist {model.Name} has been edited successfully");
            }
            return RedirectToAction(nameof(ListAll));
        }

        public IActionResult Delete(int id)
        {
            return View(id);
        }

        [Log(Operation.Delete, ArtistTable)]
        public async Task<IActionResult> Destroy(int id)
        {
            var success = await this.artistService.DeleteAsync(id);

            if (!success)
            {
                TempData.AddErrorMessage("Invalid Request");
            }
            else
            {
                TempData.AddSuccessMessage("Artist has been deleted successfully");
            }
            return RedirectToAction(nameof(ListAll));
        }
    }
}

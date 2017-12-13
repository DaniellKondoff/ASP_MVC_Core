using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Services.Contracts;
using MusicStore.Web.Models.ArtistsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Web.Controllers
{
    [Authorize]
    public class ArtistsController : Controller
    {
        private readonly IArtistService artistService;

        public ArtistsController(IArtistService artistService)
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
    }
}

using CarDealer.Services.Contracts;
using CarDealer.Web.Models.LogsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Controllers
{
    [Authorize]
    public class LogsController : Controller
    {
        private const int PageSize = 20;
        private readonly ILogService logService;

        public LogsController(ILogService logService)
        {
            this.logService = logService;
        }

        public IActionResult All(string searchString, int page = 1)
        {
            ViewData["currentFilter"] = searchString;

            var allLogs = this.logService.AllListing(page, PageSize);

            if (!String.IsNullOrEmpty(searchString))
            {
                allLogs = allLogs.Where(l => l.UserName.ToLower().Contains(searchString.ToLower()));
            }

            return View(new LogsListingViewModel
            {
                AllLogs = allLogs,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.logService.Total() / (double)PageSize)
            });
        }

        public IActionResult Clear()
        {
            this.logService.Clear();
            return RedirectToAction(nameof(All));
        }
      
    }
}

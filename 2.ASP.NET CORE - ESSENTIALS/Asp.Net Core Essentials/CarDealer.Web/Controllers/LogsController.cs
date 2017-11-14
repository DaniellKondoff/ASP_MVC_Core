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
        private const int PageSize = 2;
        private readonly ILogService logService;

        public LogsController(ILogService logService)
        {
            this.logService = logService;
        }

        public IActionResult All(string search, int page = 1)
        {
            var allLogs = this.logService.AllListing();

            if (!string.IsNullOrEmpty(search))
            {
                allLogs = allLogs.Where(l => l.UserName.ToLower().Contains(search.ToLower()));
            }

            allLogs = allLogs
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            return View(new LogsListingViewModel
            {
                AllLogs = allLogs,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.logService.Total() / (double)PageSize),
                Search = search
            });
        }

        public IActionResult Clear()
        {
            this.logService.Clear();
            return RedirectToAction(nameof(All));
        }
      
    }
}

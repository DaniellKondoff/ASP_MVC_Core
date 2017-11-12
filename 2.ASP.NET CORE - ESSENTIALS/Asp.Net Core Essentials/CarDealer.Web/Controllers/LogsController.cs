using CarDealer.Services.Contracts;
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
        private readonly ILogService logService;

        public LogsController(ILogService logService)
        {
            this.logService = logService;
        }

        public IActionResult All(string searchString)
        {
            ViewData["currentFilter"] = searchString;

            var allLogs = this.logService.AllListing();

            if (!String.IsNullOrEmpty(searchString))
            {
                allLogs = allLogs.Where(l => l.UserName.ToLower().Contains(searchString.ToLower()));
            }

            return View(allLogs);
        }

        public IActionResult Clear()
        {
            this.logService.Clear();
            return RedirectToAction(nameof(All));
        }
      
    }
}

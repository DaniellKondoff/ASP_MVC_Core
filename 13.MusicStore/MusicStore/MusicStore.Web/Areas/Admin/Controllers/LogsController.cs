using Microsoft.AspNetCore.Mvc;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Web.Areas.Admin.Models.Logs;
using System.Threading.Tasks;

namespace MusicStore.Web.Areas.Admin.Controllers
{
    public class LogsController : BaseAdminController
    {
        private readonly IAdminLogService logService;

        public LogsController(IAdminLogService logService)
        {
            this.logService = logService;
        }

        public async Task<IActionResult> ListAll(int page = 1)
        {
            var logsAll = await this.logService.AllListingAsync(page);

            return View(new LogsListingViewModel
            {
                AllLogs = logsAll,
                CurrentPage = page,
                TotalLogs = await this.logService.TotalAsync()
            });
        }

        public async Task<IActionResult> Clear()
        {
            await this.logService.ClearAsync();
            return RedirectToAction(nameof(ListAll));
        }
    }
}

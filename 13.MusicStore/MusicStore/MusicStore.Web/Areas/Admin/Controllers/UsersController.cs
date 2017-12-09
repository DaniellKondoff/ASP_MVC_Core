using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data.Models;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Web.Areas.Admin.Models.Users;
using MusicStore.Web.Infrastructure.Extensions;
using System.Linq;
using System.Threading.Tasks;
using static MusicStore.Web.Infrastructure.Common.WebConstants;

namespace MusicStore.Web.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        private readonly IAdminUserService adminService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(IAdminUserService adminService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.adminService = adminService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.adminService.AllAsync();
            var roles = await this.roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            })
            .ToListAsync();

            return this.View(new AdminUsersListingViewModel
            {
                Users = users,
                Roles = roles
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(AddUserToRoleFormModel model)
        {
            var roleExist = await this.roleManager.RoleExistsAsync(model.Role);
            var user = await this.userManager.FindByIdAsync(model.UserId);
            var userExist = user != null;

            if (!roleExist || !userExist)
            {
                ModelState.AddModelError(string.Empty, "Invalid identity details.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            await this.userManager.AddToRoleAsync(user, model.Role);

            TempData.AddSuccessMessage($"User {user.UserName} successfully added to {model.Role} role");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string UserId)
        {
            var roleExist = await this.roleManager.RoleExistsAsync(AdministratingRole);
            var user = await this.userManager.FindByIdAsync(UserId);
            var userExist = user != null;

            if (!roleExist || !userExist)
            {
                ModelState.AddModelError(string.Empty, "Invalid identity details.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var isInRole = await this.userManager.IsInRoleAsync(user, AdministratingRole);

            if (!isInRole)
            {
                TempData.AddErrorMessage($"User {user.UserName} has not been {AdministratingRole} role yet");
                return RedirectToAction(nameof(Index));
            }

            await userManager.RemoveFromRoleAsync(user, AdministratingRole);
            TempData.AddSuccessMessage($"User {user.UserName} successfully removed from {AdministratingRole} role");
            return RedirectToAction(nameof(Index));
        }
    }
}

using LearningSystem.Data.Models;
using LearningSystem.Services.Admin.Contracts;
using LearningSystem.Web.Areas.Admin.Models.Users;
using LearningSystem.Web.Common;
using LearningSystem.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSystem.Web.Areas.Admin.Controllers
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

    }
}

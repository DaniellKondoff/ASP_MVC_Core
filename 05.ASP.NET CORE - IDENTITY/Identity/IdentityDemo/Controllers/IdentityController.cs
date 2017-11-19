using IdentityDemo.Data;
using IdentityDemo.Extensions;
using IdentityDemo.Models;
using IdentityDemo.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Controllers
{
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class IdentityController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityController(ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult All()
        {
            var users = this.db.Users
                .OrderBy(u => u.Email)
                .Select(u => new ListUserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToList();

            return View(users);
        }

        public async Task<IActionResult> Roles(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var roles = await this.userManager.GetRolesAsync(user);

            return View(new UserWithRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserListModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.userManager.CreateAsync(new User
            {
                Email = model.Email,
                UserName = model.Email
            }, model.Password);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            return View(new ChangeUserPasswordViewModel
            {
                Email = user.Email
            });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string id, ChangeUserPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await this.userManager.FindByIdAsync(id);
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var result = await this.userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (result.Succeeded)
            {
                this.TempData["SuccessMessage"] = "Password has been changed successfully";
                return RedirectToAction(nameof(All));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(new DeleteUserViewModel
            {
                Id = user.Id,
                Email = user.Email
            });
        }

        public async Task<IActionResult> Destroy(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await this.userManager.DeleteAsync(user);

            TempData["SuccessMessage"] = "User Deleted";
            return RedirectToAction(nameof(All));
        }

        public IActionResult AddToRole(string id)
        {
            var rolelSelectListItems = this.roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToList();

            return View(rolelSelectListItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(string id, string role)
        {
            var user = await this.userManager.FindByIdAsync(id);
            var roleExist = await this.roleManager.RoleExistsAsync(role);

            if (user == null || !roleExist)
            {
                return NotFound();
            }

            await this.userManager.AddToRoleAsync(user, role);

            TempData["SuccessMessage"] = $"User {user.Email} added to {role} role";
            return RedirectToAction(nameof(All));
        }
    }
}

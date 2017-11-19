using CameraBazaar.Data.Models;
using CameraBazaar.Services.Contracts;
using CameraBazaar.Web.Models.AccountViewModels;
using CameraBazaar.Web.Models.UsersViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraBazaar.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public UsersController(IUserService userService, UserManager<User> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        public IActionResult Details(string id)
        {
            var user = this.userService.GetUser(id);

            return View(user);
        }

        public async Task <IActionResult> Edit(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            
            return View(new RegisterUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Phone = user.PhoneNumber             
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RegisterUserViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(id);

           
            if (user.Email != model.Email)
            {
                var emailToken = await this.userManager.GenerateChangeEmailTokenAsync(user,model.Email);
                await this.userManager.ChangeEmailAsync(user, model.Email,emailToken);
            }

            if (user.PhoneNumber != model.Phone)
            {
                var phoneToken = await this.userManager.GenerateChangePhoneNumberTokenAsync(user, model.Phone);
                await this.userManager.ChangePhoneNumberAsync(user, model.Phone, phoneToken);
            }

            var passwordToken = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var result = await this.userManager.ResetPasswordAsync(user, passwordToken, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            return RedirectToAction(nameof(CamerasController.All), "Cameras");
        }
    }
}

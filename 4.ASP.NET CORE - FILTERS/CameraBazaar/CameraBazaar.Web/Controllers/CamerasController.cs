using CameraBazaar.Data.Models;
using CameraBazaar.Services.Contracts;
using CameraBazaar.Web.Models.CamerasViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CameraBazaar.Web.Controllers
{
    [Authorize]
    public class CamerasController : Controller
    {
        private readonly ICameraService cameraService;
        private readonly UserManager<User> userManager;

        public CamerasController(ICameraService cameraService, UserManager<User> userManager)
        {
            this.cameraService = cameraService;
            this.userManager = userManager;
        }

        public IActionResult Add() => this.View();

        [HttpPost]
        public IActionResult Add(AddCameraViewModel cameraModel)
        {
            if (!ModelState.IsValid)
            {
                return View(cameraModel);
            }

            this.cameraService.Create(
                cameraModel.Make,
                cameraModel.Model,
                cameraModel.Price,
                cameraModel.Quantity,
                cameraModel.MinShutterSpeed,
                cameraModel.MaxShutterSpeed,
                cameraModel.MinISO,
                cameraModel.MaxISO,
                cameraModel.IsFullFrame,
                cameraModel.VideoResolution,
                cameraModel.LightMetering,
                cameraModel.Description,
                cameraModel.ImageUrl,
                userManager.GetUserId(this.User));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}

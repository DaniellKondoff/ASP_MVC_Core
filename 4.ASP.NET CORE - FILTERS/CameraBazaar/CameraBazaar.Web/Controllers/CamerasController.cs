using CameraBazaar.Data.Models;
using CameraBazaar.Services.Contracts;
using CameraBazaar.Services.Models.Cameras;
using CameraBazaar.Web.Infrastructure.Filters;
using CameraBazaar.Web.Models.CamerasViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        public IActionResult Add(CameraFormViewModel cameraModel)
        {
            if (cameraModel.LightMetering == null || !cameraModel.LightMetering.Any())
            {
                ModelState.AddModelError(nameof(cameraModel.LightMetering), "Please Select at least one Light Metering");
            }

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

        public IActionResult All()
        {
            var allCameras = this.cameraService.AllListing();

            return this.View(allCameras);
        }

        public IActionResult Details(int id)
        {
            var cameraDetails = this.cameraService.Details(id);

            return View(cameraDetails);
        }

        public IActionResult Edit(int id)
        {
            var camera = this.cameraService.GetById(id);
            return View(camera);
        }

        [HttpPost]
        public IActionResult Edit(int id, EditCameraServiceModel cameraModel)
        {
            if (!ModelState.IsValid)
            {
                return View(cameraModel);
            }

            this.cameraService.Edit(
                id,
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
                cameraModel.ImageUrl
                );

            return  RedirectToAction(nameof(All));
        }

        public IActionResult Delete(int id)
        {
            this.cameraService.Delete(id);
            return RedirectToAction(nameof(All));
        }
    }
}

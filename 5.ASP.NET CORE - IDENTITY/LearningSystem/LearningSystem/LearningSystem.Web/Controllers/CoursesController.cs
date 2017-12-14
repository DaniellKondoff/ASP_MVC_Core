using LearningSystem.Data;
using LearningSystem.Data.Models;
using LearningSystem.Services.Contracts;
using LearningSystem.Services.Models;
using LearningSystem.Web.Infrastructure.Extensions;
using LearningSystem.Web.Models.CoursesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearningSystem.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly UserManager<User> userManager;

        public CoursesController(ICourseService courseService, UserManager<User> userManager)
        {
            this.courseService = courseService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = new CourseDetailsViewModel
            {
                CourseDetails = await this.courseService.ByIdAsync<CourseDetailsServiceModel>(id)
            };

            if (model.CourseDetails == null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userId = this.userManager.GetUserId(User);
                model.UserisSignedInCourse =
                    await this.courseService.StudentIsSignedInCourseAsync(id, userId);
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignIn(int id)
        {
            var userId = this.userManager.GetUserId(User);

            var success = await this.courseService.SignInStudentAsync(id, userId);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Thank you for you registration!");

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOut(int id)
        {
            var userId = this.userManager.GetUserId(User);

            var success = await this.courseService.SignOutStudentAsync(id, userId);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("You have signed out course successfully");

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitExam(IFormFile exam, int id)
        {
            if (!exam.FileName.EndsWith(".zip") || exam.Length > DataConstants.CourseExamSUbmissionFileLenght)
            {
                TempData.AddErrorMessage("Your submission should be a 'zip' file with no more than 2MB in size");

                return RedirectToAction(nameof(Details), new { id });
            }

            var fileContents = await exam.ToByteArrayAsync();

            var userId = this.userManager.GetUserId(User);

            var success = await this.courseService.SaveExamSubmission(id, userId, fileContents);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Exam SUbmissions Saved");
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}

using LearningSystem.Data.Models;
using LearningSystem.Services.Contracts;
using LearningSystem.Services.Models;
using LearningSystem.Web.Common;
using LearningSystem.Web.Models.TrainersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSystem.Web.Controllers
{
    [Authorize(Roles =WebConstants.TrainerRole)]
    public class TrainersController : Controller
    {
        private readonly ITrainerService trainerService;
        private readonly UserManager<User> userManager;
        private readonly ICourseService courseService;

        public TrainersController(ITrainerService trainerService, UserManager<User> userManager, ICourseService courseService)
        {
            this.trainerService = trainerService;
            this.userManager = userManager;
            this.courseService = courseService;
        }

        public async Task<IActionResult> Courses()
        {
            var trainerId = this.userManager.GetUserId(User);
            var courses = await this.trainerService.CoursesByIdAsync(trainerId);

            return View(courses);
        }

        public async Task<IActionResult> Students(int id)
        {
            var trainerId = this.userManager.GetUserId(User);

            if (!await this.trainerService.IsTrainerCourse(id,trainerId))
            {
                return BadRequest();
            }

            var students = await this.trainerService.StudentsInCourseAsync(id);


            return View(new StudentsInCourseViewModel
            {
                Students = students,
                CourseByStudent = await this.courseService.ByIdAsync<CourseListingServiceModel>(id)
            });
        }

        [HttpPost]
        public async Task<IActionResult> GradeStudent(int id, string studentId, Grade grade)
        {
            if (studentId == null)
            {
              return  BadRequest();
            }
            var trainerId = this.userManager.GetUserId(User);
            if (!await this.trainerService.IsTrainerCourse(id, trainerId))
            {
                return BadRequest();
            }

            var success = await this.trainerService.AddGrade(id, studentId, grade);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Students), new { id });
        }
    }
}

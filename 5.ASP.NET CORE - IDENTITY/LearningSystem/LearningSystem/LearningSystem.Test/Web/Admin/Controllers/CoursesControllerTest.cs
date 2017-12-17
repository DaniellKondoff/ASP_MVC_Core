namespace LearningSystem.Test.Web.Admin.Controllers
{
    using FluentAssertions;
    using LearningSystem.Data.Models;
    using LearningSystem.Services.Admin.Contracts;
    using LearningSystem.Test.Mocks;
    using LearningSystem.Web.Areas.Admin.Controllers;
    using LearningSystem.Web.Areas.Admin.Models.Courses;
    using LearningSystem.Web.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class CoursesControllerTest
    {
        private const string FirstUserId = "1";
        private const string FirstUserName = "First";
        private const string SecondUserId = "2";
        private const string SecondUserName = "Second";


        [Fact]
        public void CoursesControllerShouldBeInAdminArea()
        {
            //Arrenge
            var controller = typeof(CoursesController);

            //Act
            var areaAttribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute))
                as AreaAttribute;

            //Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.RouteValue.Should().Be(WebConstants.AdminArea);
        }

        [Fact]
        public void CoursesControllerShouldBeOnlyInAdminRole()
        {
            //Arrenge
            var controller = typeof(CoursesController);

            //Act
            var areaAttribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            //Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.Roles.Should().Be(WebConstants.AdministratingRole);
        }

        [Fact]
        public async Task GetCreateShouldReturnVIewWithValidModel()
        {
            //Arrenge
            var userManager = this.GetUserManagerMock();

            var controller = new CoursesController(userManager.Object, null);

            //Act
            var result = await controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();

            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<AddCourseFormModel>();

            var formModel = model.As<AddCourseFormModel>();

            formModel.StartDate.Year.Should().Be(DateTime.UtcNow.Year);
            formModel.StartDate.Month.Should().Be(DateTime.UtcNow.Month);
            formModel.StartDate.Day.Should().Be(DateTime.UtcNow.Day);

            var endDate = DateTime.UtcNow.AddDays(30);

            formModel.EndDate.Year.Should().Be(endDate.Year);
            formModel.EndDate.Month.Should().Be(endDate.Month);
            formModel.EndDate.Day.Should().Be(endDate.Day);

            formModel.Trainers.Should().Match(items => items.Count() == 2);
            this.AssertTrainersSelectListItems(formModel.Trainers);
        }

        [Fact]
        public async Task PostCreateShouldReturnViewWithCorrectMethodWhenModelStateIsInvalid()
        {
            //Arrenge
            var userManager = this.GetUserManagerMock();
            var controller = new CoursesController(userManager.Object, null);
            controller.ModelState.AddModelError(string.Empty, "Error");

            //Act
            var result = await controller.Create(new AddCourseFormModel());

            //Assert
            result.Should().BeOfType<ViewResult>();

            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<AddCourseFormModel>();

            var formModel = model.As<AddCourseFormModel>();

            this.AssertTrainersSelectListItems(formModel.Trainers);
        }

        [Fact]
        public async Task PostCreateShouldReturnRedirectWithValidModel()
        {
            //Arrenge
            string modelName = null;
            string modelDescription = null;
            DateTime modelStartDate = DateTime.UtcNow;
            DateTime modelEndDate = DateTime.UtcNow;
            string modelTrainerId  = null;
            string successMessage = null;

            var adminCourseService = new Mock<IAdminCourseService>();
            adminCourseService
                .Setup(c => c.CreateAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>()))
                 .Callback((string name, string description, DateTime startdate, DateTime endDate, string trainerId) =>
                 {
                     modelName = name;
                     modelDescription = description;
                     modelStartDate = startdate;
                     modelEndDate = endDate;
                     modelTrainerId = trainerId;
                 })
                 .Returns(Task.CompletedTask);

            var tempDate = new Mock<ITempDataDictionary>();
            tempDate.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object message) => successMessage = message as string);

            var controller = new CoursesController(null, adminCourseService.Object);
            controller.TempData = tempDate.Object;

            //Ask
            var result = await controller.Create(new AddCourseFormModel
            {
                Name = "Name",
                Description = "Description",
                StartDate = new DateTime(2000, 12, 10),
                EndDate = new DateTime(2000,12, 15),
                TrainerId = "1"
            });

            //Assert
            modelName.Should().Be("Name");
            modelDescription.Should().Be("Description");
            modelStartDate.Should().Be(new DateTime(2000, 12, 10));
            modelEndDate.Should().Be(new DateTime(2000, 12, 15).AddDays(1));
            modelTrainerId.Should().Be("1");

            successMessage.Should().Be($"Course Name created successfully!");

            result.Should().BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Index");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Home");
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
            result.As<RedirectToActionResult>().RouteValues.Values.Should().Contain(string.Empty);
        }

        private Mock<UserManager<User>> GetUserManagerMock()
        {
           var userManager = UserManagerMock.New();
            userManager
                .Setup(u => u.GetUsersInRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<User>
                {
                    new User { Id = FirstUserId, UserName = FirstUserName },
                    new User { Id = SecondUserId, UserName = SecondUserName }
                });

            return userManager;
        }

        private void AssertTrainersSelectListItems(IEnumerable<SelectListItem> trainers)
        {
            trainers.First().Should().Match(u => u.As<SelectListItem>().Value == FirstUserId);
            trainers.Last().Should().Match(u => u.As<SelectListItem>().Value == SecondUserId);
            trainers.First().Should().Match(u => u.As<SelectListItem>().Text == FirstUserName);
            trainers.Last().Should().Match(u => u.As<SelectListItem>().Text == SecondUserName);
        }

    }
}

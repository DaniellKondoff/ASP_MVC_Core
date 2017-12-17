namespace LearningSystem.Test.Web.Controllers
{
    using LearningSystem.Web.Controllers;
    using Xunit;
    using FluentAssertions;
    using System.Linq;
    using Moq;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using LearningSystem.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using LearningSystem.Services.Contracts;
    using LearningSystem.Services.Models;
    using LearningSystem.Test.Mocks;

    public class UserControllerTest
    {
        [Fact]
        public void DownloadCertificateShouldBeOnlyAuthorizedUsers()
        {
            //Arrenge
            var method = typeof(UsersController).GetMethod(nameof(UsersController.DownloadCertificate));

            //Act
            var attributes = method.GetCustomAttributes(true);

            //Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public async Task ProfileShouldReturnNotFoundWithInvalidUserName()
        {
            //Arrenge
            var userManager = UserManagerMock.New();

            var controller = new UsersController(null, userManager.Object);

            //Act

            var result = await controller.Profile("Username");

            //Assert
            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ProfileShouldReturnViewWithValidModelWithValidUserName()
        {
            //Arrenge
            const string userId = "SomeId";
            const string userName = "SomeUsername";

            var userManager = UserManagerMock.New();

            userManager
                .Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Id = userId});

            var userService = new Mock<IUserService>();
            userService
                .Setup(u => u.ProfileAsync(It.Is<string>(id => id == userId)))
                .ReturnsAsync(new UserProfileServiceModel { UserName = userName });

            var controller = new UsersController(userService.Object, userManager.Object);

            //Act
            var result = await controller.Profile(userName);

            //Assert

            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<UserProfileServiceModel>().UserName == userName);
                
        }

    }
}

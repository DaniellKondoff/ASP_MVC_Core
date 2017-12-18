using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using MusicStore.Data.Models;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Services.Admin.Models.Artists;
using MusicStore.Web.Areas.Admin.Controllers;
using MusicStore.Web.Areas.Admin.Models.Songs;
using MusicStore.Web.Infrastructure.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicStore.Tests.Web.Areas.Admin.Controllers
{
    public class SongControllerTest
    {
        private const int FirstUserId = 1;
        private const string FirstUserName = "First";
        private const int SecondUserId = 20;
        private const string SecondUserName = "Second";

        [Fact]
        public void SongController_ShoulbBeInAdminArea()
        {
            //Arrenge
            var controller = typeof(SongsController);

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
        public void SongController_ShoulbBeInAdminRole()
        {
            //Arrenge
            var controller = typeof(SongsController);

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
        public async Task SongController_GetAdd_ShouldReturnViewWithValidModel()
        {
            //Arrenge
            var adminArtistService = this.GetAdminArtistServiceMock();

            var controller = new SongsController(adminArtistService.Object, null);

            //Act
            var result = await controller.Add();

            //Assert
            result.Should().BeOfType<ViewResult>();

            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<SongFormViewModel>();

            var formModel = model.As<SongFormViewModel>();

            this.AssertArtistsSelectListItems(formModel.Artists);
        }

        [Fact]
        public async Task SongController_PostAdd_ShouldReturnViewWithCorrectMethodWhenModelStateIsInvalid()
        {
            //Arrenge
            var adminArtistService = this.GetAdminArtistServiceMock();
            var controller = new SongsController(adminArtistService.Object, null);
            controller.ModelState.AddModelError(string.Empty, "Error");

            //Act
            var result = await controller.Add(new SongFormViewModel());

            //Assert
            result.Should().BeOfType<ViewResult>();
            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<SongFormViewModel>();

            var formModel = model.As<SongFormViewModel>();

            this.AssertArtistsSelectListItems(formModel.Artists);
        }

        [Fact]
        public async Task SongController_PostAdd_ShouldReturnRedirectWithValidModel()
        {
            //Arrenge
            string modelName = null;
            decimal modelPrice = 0;
            double modelDuration = 0;
            int modelArtistId = 0;
            Ganre modelGanre = 0;
            string successMessage = null;

            string resultModelName = "TestSong";
            decimal resultModelPrice = 2;
            double resultModelDuration = 3;
            int resultModelArtistId = 4;
            Ganre resultModelGanre = Ganre.Disco;

            var adminSongService = new Mock<IAdminSongService>();
            adminSongService
                .Setup(s => s.CreateAsync(
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<double>(),
                    It.IsAny<int>(),
                    It.IsAny<Ganre>()))
                .Callback((string name, decimal price, double duration, int artistId, Ganre ganre) =>
                {
                    modelName = name;
                    modelPrice = price;
                    modelDuration = duration;
                    modelArtistId = artistId;
                    modelGanre = ganre;
                })
                .Returns(Task.CompletedTask);

            var adminArtistService = new Mock<IAdminArtistService>();

            adminArtistService
                .Setup(a => a.ExistAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var tempDate = new Mock<ITempDataDictionary>();
            tempDate.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object message) => successMessage = message as string);

            var controller = new SongsController(adminArtistService.Object, adminSongService.Object);
            controller.TempData = tempDate.Object;

            //Act
            var result = await controller.Add(new SongFormViewModel
            {
                Name = resultModelName,
                Price = resultModelPrice,
                Duration = resultModelDuration,
                ArtistId = resultModelArtistId,
                Ganre = resultModelGanre
            });

            //Assert
            modelName.Should().Be(resultModelName);
            modelPrice.Should().Be(resultModelPrice);
            modelDuration.Should().Be(resultModelDuration);
            modelArtistId.Should().Be(resultModelArtistId);
            modelGanre.Should().Be(resultModelGanre);

            successMessage.Should().Be($"The song {resultModelName} has been added successfully");

            result.Should().BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("ListAll");
        }

        [Fact]
        public async Task SongController_Destroy_ShouldReturnRedirectWithSuccessTempData()
        {
            //Arrenge
            string successMessage = null;
            int songId = 1;

            var songService = new Mock<IAdminSongService>();
            songService
                .Setup(s => s.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var tempDate = new Mock<ITempDataDictionary>();

            tempDate.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object message) => successMessage = message as string);

            var controller = new SongsController(null, songService.Object);
            controller.TempData = tempDate.Object;

            //Act
            var result = await controller.Destroy(songId);

            //Assert
            successMessage.Should().Be("Song has been deleted successfully");

            result.Should().BeOfType<RedirectToActionResult>();
            result.As<RedirectToActionResult>().ActionName.Should().Be("ListAll");
        }

        private Mock<IAdminArtistService> GetAdminArtistServiceMock()
        {
            var adminArtistService = new Mock<IAdminArtistService>();

            adminArtistService
                .Setup(a => a.AllBasicAsync())
                .ReturnsAsync(new List<AdminBaseArtistServiceModel>
                {
                    new AdminBaseArtistServiceModel{ Id = FirstUserId, Name = FirstUserName},
                    new AdminBaseArtistServiceModel{ Id = SecondUserId, Name = SecondUserName}
                });

            return adminArtistService;
        }
        private void AssertArtistsSelectListItems(IEnumerable<SelectListItem> artists)
        {
            artists.Should().Match(a => a.Count() == 2);
            artists.First().Should().Match(a => a.As<SelectListItem>().Value == FirstUserId.ToString());
            artists.Last().Should().Match(a => a.As<SelectListItem>().Value == SecondUserId.ToString());
            artists.First().Should().Match(a => a.As<SelectListItem>().Text == FirstUserName);
            artists.Last().Should().Match(a => a.As<SelectListItem>().Text == SecondUserName);
        }
    }
}

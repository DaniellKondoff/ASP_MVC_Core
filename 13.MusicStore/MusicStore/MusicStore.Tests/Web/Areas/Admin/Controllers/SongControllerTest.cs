using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using MusicStore.Data.Models;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Services.Admin.Models.Artists;
using MusicStore.Services.Admin.Models.Songs;
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
        private const string FirstArtistName = "FirstArtist";
        private const int SecondUserId = 20;
        private const string SecondUserName = "Second";
        private const string SecondArtistName = "SecondArtist";


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
            var adminSongService = this.GetAdminSongServiceBaseMock();
            var controller = new SongsController(adminArtistService.Object, adminSongService.Object);
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

            var adminSongService = this.GetAdminSongServiceBaseMock();
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

            adminSongService
                .Setup(a => a.IsGanreExist(It.IsAny<int>()))
                .Returns(true);

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

            var songService = this.GetAdminSongServiceBaseMock();
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

        [Fact]
        public async Task SongController_GetEdit_ShouldReturnBadRequestWhenSongIsNull()
        {
            //Arrenge
            var adminSongService = this.GetAdminSongServiceBaseMock();
            adminSongService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((AdminSongEditServiceModel)null);

            var controller = new SongsController(null, adminSongService.Object);
            //Act
            var result = await controller.Edit(1);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task SongController_GetEdit_ShouldReturnViewWithValidModel()
        {
            const string SongName = "TestSong";
            const double SongDuration = 2;
            const decimal SongPrice = 5;
            const string SongArtist = "ArtistName";
            //Arrenge
            var adminArtistService = this.GetAdminArtistServiceMock();
            var adminSongService = this.GetAdminSongServiceBaseMock();
            adminSongService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new AdminSongEditServiceModel
                {
                    Name = SongName,
                    Duration = SongDuration,
                    Price = SongPrice,
                    Artist = SongArtist
                });

            var controller = new SongsController(adminArtistService.Object, adminSongService.Object);

            //Act
            var result = await controller.Edit(1);


            //Assert
            result.Should().BeOfType<ViewResult>();
            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<SongFormViewModel>();

            var formModel = model.As<SongFormViewModel>();

            formModel.Name.Should().Be(SongName);
            formModel.Duration.Should().Be(SongDuration);
            formModel.Price.Should().Be(SongPrice);

            this.AssertArtistsSelectListItems(formModel.Artists);
        }

        [Fact]
        public async Task SongController_PostEdit_ShouldReuturnViewWithCorrectMethodWhenModelStateIsInvalid()
        {
            //Arrenge
            const string SongName = "TestSong";
            const double SongDuration = 2;
            const decimal SongPrice = 5;

            var adminSongService = this.GetAdminSongServiceBaseMock();
            adminSongService.Setup(s => s.ExistAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var adminArtistService = this.GetAdminArtistServiceMock();

            var controller = new SongsController(adminArtistService.Object, adminSongService.Object);
            //Act
            var result = await controller.Edit(2, new SongFormViewModel
            {
                Name = SongName,
                Duration = SongDuration,
                Price = SongPrice,
                Ganre = Ganre.Blues,
                ArtistId = 1
            });

            //Assert
            result.Should().BeOfType<ViewResult>();
            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<SongFormViewModel>();

            var formModel = model.As<SongFormViewModel>();

            formModel.Name.Should().Be(SongName);
            formModel.Duration.Should().Be(SongDuration);
            formModel.Price.Should().Be(SongPrice);

            this.AssertArtistsSelectListItems(formModel.Artists);

        }

        [Fact]
        public async Task SongController_PostEdit_ShoudReturnRedirectWithViewModel()
        {
            //Arrenge
            int modelId = 0;
            string modelName = null;
            decimal modelPrice = 0;
            double modelDuration = 0;
            int modelArtistId = 0;
            Ganre modelGanre = 0;
            string successMessage = null;

            int resultModelId = 1;
            string resultModelName = "TestSong";
            decimal resultModelPrice = 2;
            double resultModelDuration = 3;
            int resultModelArtistId = 4;
            Ganre resultModelGanre = Ganre.Disco;

            var adminSongService = this.GetAdminSongServiceBaseMock();
            adminSongService.Setup(s => s.ExistAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var adminArtistService = this.GetAdminArtistServiceMock();
            adminArtistService.Setup(a => a.ExistAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            adminSongService.Setup(s => s.EditAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<double>(),
                It.IsAny<int>(),
                It.IsAny<Ganre>()
                ))
                .Callback((int id, string name, decimal price, double duration, int artistId, Ganre ganre) =>
                {
                    modelId = id;
                    modelName = name;
                    modelPrice = price;
                    modelDuration = duration;
                    modelArtistId = artistId;
                    modelGanre = ganre;
                })
                .Returns(Task.CompletedTask);

            adminSongService
                .Setup(a => a.IsGanreExist(It.IsAny<int>()))
                .Returns(true);

            var tempDate = new Mock<ITempDataDictionary>();
            tempDate.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object message) => successMessage = message as string);

            var controller = new SongsController(adminArtistService.Object, adminSongService.Object);
            controller.TempData = tempDate.Object;

            //Act
            var result = await controller.Edit(resultModelId, new SongFormViewModel
            {
                Name = resultModelName,
                Price = resultModelPrice,
                Duration = resultModelDuration,
                ArtistId = resultModelArtistId,
                Ganre = resultModelGanre
            });

            //Assert
            modelId.Should().Be(resultModelId);
            modelName.Should().Be(resultModelName);
            modelPrice.Should().Be(resultModelPrice);
            modelDuration.Should().Be(resultModelDuration);
            modelArtistId.Should().Be(resultModelArtistId);
            modelGanre.Should().Be(resultModelGanre);

            successMessage.Should().Be($" Song {resultModelName} has been edited successfully");

            result.Should().BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("ListAll");
        }

        [Fact]
        public async Task SongController_GetDetails_ShouldReturnBadRequestWhenSongDoesNotExist()
        {
            //Arrenge
            var adminSongService = this.GetAdminSongServiceBaseMock();
            adminSongService
                .Setup(s => s.DetailsAsync(It.IsAny<int>()))
                .ReturnsAsync((AdminSongDetailsServiceModel)null);

            var controller = new SongsController(null, adminSongService.Object);
            //Act
            var result = await controller.Details(10);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task SongController_GetDetails_ShouldReturnViewWithViewModel()
        {
            //Arrenge
            const int SongId = 10;
            const string SongName = "TestSong";
            const double SongDuration = 2;
            const decimal SongPrice = 5;
            const string SongArtist = "TestArtist";


            var adminSongService = this.GetAdminSongServiceBaseMock();
            adminSongService
                .Setup(s => s.DetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new AdminSongDetailsServiceModel
                {
                    Id = SongId,
                    Name = SongName,
                    Duration = SongDuration,
                    Price = SongPrice,
                    Artist = SongArtist,
                    Ganre = Ganre.Blues
                });

            var controller = new SongsController(null, adminSongService.Object);
            //Act
            var result = await controller.Details(SongId);

            //Assert
            result.Should().BeOfType<ViewResult>();
            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<AdminSongDetailsServiceModel>();

            var formModel = model.As<AdminSongDetailsServiceModel>();

            formModel.Id.Should().Be(SongId);
            formModel.Name.Should().Be(SongName);
            formModel.Duration.Should().Be(SongDuration);
            formModel.Price.Should().Be(SongPrice);
            formModel.Artist.Should().Be(SongArtist);
            formModel.Ganre.Should().Be(Ganre.Blues);
        }

        [Fact]
        public async Task SongController_ListAll_ShouldreturnViewWithSongs()
        {
            //Arrange
            const int TotalSongs = 2;
            var adminSongService = this.GetAdminSongServiceBaseMock();
            adminSongService
                .Setup(s => s.AllAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<AdminSongListingServiceModel>
                {
                    new AdminSongListingServiceModel { Id = FirstUserId, Name = FirstUserName, Artist= FirstArtistName},
                    new AdminSongListingServiceModel { Id = SecondUserId, Name = SecondUserName, Artist= SecondArtistName}
                });

            adminSongService
                .Setup(s => s.TotalAsync())
                .ReturnsAsync(TotalSongs);

            var controller = new SongsController(null, adminSongService.Object);

            //Act
            var result = await controller.ListAll(1);

            //Assert
            result.Should().BeOfType<ViewResult>();
            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<SongListingViewModel>();

            var formModel = model.As<SongListingViewModel>();

            formModel.AllSongs.Should().Match(s => s.Count() == 2);
            formModel.CurrentPage.Should().Be(1);
            formModel.TotalSongs.Should().Be(TotalSongs);

            formModel.AllSongs.First().Should().Match(s => s.As<AdminSongListingServiceModel>().Id == FirstUserId);
            formModel.AllSongs.First().Should().Match(s => s.As<AdminSongListingServiceModel>().Name == FirstUserName);
            formModel.AllSongs.First().Should().Match(s => s.As<AdminSongListingServiceModel>().Artist == FirstArtistName);
            formModel.AllSongs.Last().Should().Match(s => s.As<AdminSongListingServiceModel>().Id == SecondUserId);
            formModel.AllSongs.Last().Should().Match(s => s.As<AdminSongListingServiceModel>().Name == SecondUserName);
            formModel.AllSongs.Last().Should().Match(s => s.As<AdminSongListingServiceModel>().Artist == SecondArtistName);
        }

        private Mock<IAdminSongService> GetAdminSongServiceBaseMock()
        {
            return new Mock<IAdminSongService>();
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

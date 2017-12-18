using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicStore.Services.Contracts;
using MusicStore.Services.Models.Albums;
using MusicStore.Services.Models.Artists;
using MusicStore.Services.Models.Songs;
using MusicStore.Web.Controllers;
using MusicStore.Web.Models.HomeViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicStore.Tests.Web.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public async Task HomeControllerSearch_ShouldReturnNoResultsWithNoCreteria()
        {
            //Arrenge
            var controller = new HomeController(null, null, null);

            //Act
            var result = await controller.Search(new SearchFormModel
            {
                SearchInAlbums = false,
                SearchInArtists = false,
                SearchInSongs = false
            });

            //Assert
            result.Should().BeOfType<ViewResult>();

            result.As<ViewResult>().Model.Should().BeOfType<SearchViewModel>();

            var searchViewModel = result.As<ViewResult>().Model.As<SearchViewModel>();

            searchViewModel.Albums.Should().BeEmpty();
            searchViewModel.Artists.Should().BeEmpty();
            searchViewModel.Songs.Should().BeEmpty();
            searchViewModel.SearchText.Should().BeNull();
        }

        [Fact]
        public async Task HomeControllerSearch_ShouldReturnViewWithValidModelWhenArtistAreFiltered()
        {
            //Arrenge
            const string searchText = "TextArtist";
            var artistService = new Mock<IArtistService>();
            artistService.Setup(a => a.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ArtistListingServiceModel>
                {
                    new ArtistListingServiceModel{Id = 1},
                    new ArtistListingServiceModel{Id = 2}
                });

            var controller = new HomeController(artistService.Object, null, null);

            //Act
            var result = await controller.Search(new SearchFormModel
            {
                SearchInAlbums = false,
                SearchInArtists = true,
                SearchInSongs = false,
                SearchText = searchText
            });

            //Assert
            result.Should().BeOfType<ViewResult>();

            result.As<ViewResult>().Model.Should().BeOfType<SearchViewModel>();

            var searchViewModel = result.As<ViewResult>().Model.As<SearchViewModel>();

            searchViewModel.Albums.Should().BeEmpty();
            searchViewModel.Songs.Should().BeEmpty();
            searchViewModel.SearchText.Should().Be(searchText);

            searchViewModel.Artists.Should().Match(a => a.As<List<ArtistListingServiceModel>>().Count == 2);
            searchViewModel.Artists.First().Should().Match(a => a.As<ArtistListingServiceModel>().Id == 1);
            searchViewModel.Artists.Last().Should().Match(a => a.As<ArtistListingServiceModel>().Id == 2);
        }

        [Fact]
        public async Task HomeControllerSearch_ShouldReturnViewWithValidModelWhenAlbumAreFiltered()
        {
            //Arrenge
            const string searchText = "TextAlbum";
            var albumService = new Mock<IAlbumService>();
            albumService.Setup(a => a.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<AlbumsListingServiceModel>
                {
                    new AlbumsListingServiceModel{Id = 1},
                    new AlbumsListingServiceModel{Id = 2}
                });

            var controller = new HomeController( null, null, albumService.Object);

            //Act
            var result = await controller.Search(new SearchFormModel
            {
                SearchInAlbums = true,
                SearchInArtists = false,
                SearchInSongs = false,
                SearchText = searchText
            });

            //Assert
            result.Should().BeOfType<ViewResult>();

            result.As<ViewResult>().Model.Should().BeOfType<SearchViewModel>();

            var searchViewModel = result.As<ViewResult>().Model.As<SearchViewModel>();

            searchViewModel.Artists.Should().BeEmpty();
            searchViewModel.Songs.Should().BeEmpty();
            searchViewModel.SearchText.Should().Be(searchText);

            searchViewModel.Albums.Should().Match(a => a.As<List<AlbumsListingServiceModel>>().Count == 2);
            searchViewModel.Albums.First().Should().Match(a => a.As<AlbumsListingServiceModel>().Id == 1);
            searchViewModel.Albums.Last().Should().Match(a => a.As<AlbumsListingServiceModel>().Id == 2);
        }

        [Fact]
        public async Task HomeControllerSearch_ShouldReturnViewWithValidModelWhenSongAreFiltered()
        {
            //Arrenge
            const string searchText = "TextSong";
            var songService = new Mock<ISongService>();
            songService.Setup(a => a.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<SongListingServiceModel>
                {
                    new SongListingServiceModel{Id = 1},
                    new SongListingServiceModel{Id = 2}
                });

            var controller = new HomeController(null, songService.Object, null);

            //Act
            var result = await controller.Search(new SearchFormModel
            {
                SearchInAlbums = false,
                SearchInArtists = false,
                SearchInSongs = true,
                SearchText = searchText
            });

            //Assert
            result.Should().BeOfType<ViewResult>();

            result.As<ViewResult>().Model.Should().BeOfType<SearchViewModel>();

            var searchViewModel = result.As<ViewResult>().Model.As<SearchViewModel>();

            searchViewModel.Artists.Should().BeEmpty();
            searchViewModel.Albums.Should().BeEmpty();
            searchViewModel.SearchText.Should().Be(searchText);

            searchViewModel.Songs.Should().Match(a => a.As<List<SongListingServiceModel>>().Count == 2);
            searchViewModel.Songs.First().Should().Match(a => a.As<SongListingServiceModel>().Id == 1);
            searchViewModel.Songs.Last().Should().Match(a => a.As<SongListingServiceModel>().Id == 2);
        }
    }
}

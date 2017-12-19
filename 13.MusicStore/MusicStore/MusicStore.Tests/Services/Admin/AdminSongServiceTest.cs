using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Data.Models;
using MusicStore.Services.Admin.Implementations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicStore.Tests.Services.Admin
{
    public class AdminSongServiceTest
    {
        private const int SongId = 1;
        private const string SongName = "SongName";
        private const string EditedSongName = "EditedSongName";
        private const decimal SongPrice = 5.4m;
        private const double SongDuration = 3.14;
        private const int SongArtistId = 1;
        private const string ArtistName = "FirstArtist";
        private const Ganre SongGanre = Ganre.Blues;
        private const int TotalSongsCount = 3;

        public AdminSongServiceTest()
        {
            TestStartUp.Initialize();
        }

        [Fact]
        public async Task AdminSongService_CreateAsync_ShouldCreateAndSaveCorrectSong()
        {
            //Arrenge
            var db = this.GetDatabase();

            var adminSongService = new AdminSongService(db);

            //Act
            await adminSongService.CreateAsync(SongName, SongPrice, SongDuration, SongArtistId, SongGanre);
            var savedSong = await db.Songs.FirstOrDefaultAsync();

            //Assert
            savedSong.Should().NotBeNull();
            savedSong.Id.Should().Be(SongId);
            savedSong.Name.Should().Be(SongName);
            savedSong.Price.Should().Be(SongPrice);
            savedSong.Duration.Should().Be(SongDuration);
            savedSong.ArtistId.Should().Be(SongArtistId);
            savedSong.Ganre.Should().Be(SongGanre);
        }

        [Fact]
        public async Task AdminSongService_DeleteAsync_ShouldDeleteAndSaveCorrectSong()
        {
            //Arrenge
            var db = this.GetDatabase();
            await this.PopulateDbAsync(db);

            var adminSongService = new AdminSongService(db);
            var song = await db.Songs.FindAsync(SongId);
            //Act

            var result = await adminSongService.DeleteAsync(SongId);

            //Assert
            result.Should().Be(true);
            db.Songs.Should().NotContain(song);
            song.Should().NotBeNull();
        }

        [Fact]
        public async Task AdminSongService_DeleteAsync_ShouldReturnFalseWhenSongDoesNotExist()
        {
            //Arrenge
            var db = this.GetDatabase();
            await this.PopulateDbAsync(db);

            var adminSongService = new AdminSongService(db);
            var song = await db.Songs.FindAsync(SongId + 10);
            //Act

            var result = await adminSongService.DeleteAsync(SongId + 10);

            //Assert
            result.Should().Be(false);
            song.Should().BeNull();
        }

        [Fact]
        public async Task AdminSongService_ExistAsync_ShouldReturnTrueForValidSong()
        {
            //Arrenge
            var db = this.GetDatabase();
            await this.PopulateDbAsync(db);


            var adminSongService = new AdminSongService(db);
            //Act

            var result = await adminSongService.ExistAsync(SongId);

            //Assert
            result.Should().Be(true);

        }

        [Fact]
        public async Task AdminSongService_TotalAsync_ShouldReturnTotalSongs()
        {
            //Arrenge
            var db = this.GetDatabase();
            await this.PopulateDbAsync(db);

            var adminSongService = new AdminSongService(db);

            //Act
            var result = await adminSongService.TotalAsync();

            //Assert
            result.Should().Be(TotalSongsCount);
        }

        [Fact]
        public async Task AdminSongService_EditAsync_ShouldEditWhenSongsExist()
        {
            //Arrenge
            var db = this.GetDatabase();
            await this.PopulateDbAsync(db);

            var adminSongService = new AdminSongService(db);

            //Act
            await adminSongService.EditAsync(SongId, EditedSongName, SongPrice, SongDuration, SongArtistId, SongGanre);
            var editedSong = await db.Songs.FindAsync(SongId);

            //Assert
            editedSong.Should().NotBeNull();
            editedSong.Id.Should().Be(SongId);
            editedSong.Name.Should().Be(EditedSongName);
            editedSong.Price.Should().Be(SongPrice);
            editedSong.Duration.Should().Be(SongDuration);
            editedSong.ArtistId.Should().Be(SongArtistId);
            editedSong.Ganre.Should().Be(SongGanre);
        }

        [Fact]
        public async Task AdminSongService_EditAsync_ShouldDoNotEditWhenSongsDoesNotExist()
        {
            //Arrenge
            var db = this.GetDatabase();
            await this.PopulateDbAsync(db);

            var adminSongService = new AdminSongService(db);

            //Act
            await adminSongService.EditAsync(10, EditedSongName, SongPrice, SongDuration, SongArtistId, SongGanre);
            var editedSong = await db.Songs.FindAsync(10);

            //Assert
            editedSong.Should().BeNull();
        }

        [Fact]
        public async Task AdminSongService_GetByIdAsync_ShouldReturnTrueForValidSong()
        {
            //Arrenge
            var db = this.GetDatabase();
            await this.PopulateDbAsync(db);


            var adminSongService = new AdminSongService(db);
            //Act

            var result = await adminSongService.GetByIdAsync(SongId);

            //Assert
            result.Name.Should().Be(SongName);
            result.Price.Should().Be(SongPrice);
            result.Duration.Should().Be(SongDuration);
            result.Artist.Should().Be(ArtistName);

        }

        [Fact]
        public async Task AdminSongService_AllAsync_ShouldReturnCollectionOfSongs()
        {
            //Arrenge
            var db = this.GetDatabase();
            await this.PopulateDbAsync(db);


            var adminSongService = new AdminSongService(db);
            //Act

            var result = await adminSongService.AllAsync();

            //Assert
            result
                 .Should()
                 .Match(s => s.ElementAt(0).Id == 3 && s.ElementAt(2).Id == 1)
                 .And
                 .HaveCount(3);
        }

        [Fact]
        public async Task AdminSongService_AllAsyncBasic_ShouldReturnValidSongs()
        {
            //Arrenge
            var db = this.GetDatabase();
            await this.PopulateDbAsync(db);


            var adminSongService = new AdminSongService(db);
            //Act

            var result = await adminSongService.AllBasicAsync(SongArtistId);

            //Assert
            result
                .Should()
                .HaveCount(1)
                .And
                .Match(s => s.Last().Name == SongName);
        }

        private MusicStoreDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<MusicStoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MusicStoreDbContext(dbOptions);
        }

        private async Task PopulateDbAsync(MusicStoreDbContext db)
        {
            var firstSong = new Song { Id = 1, Name = SongName, Price = SongPrice, Duration = SongDuration, ArtistId = SongArtistId };
            var secondSong = new Song { Id = 2, Name = "SecondSong", Price = 2.5m, ArtistId = 2 };
            var thirdSong = new Song { Id = 3, Name = "ThirdSong", Price = 9.5m, ArtistId = 3 };

            var firstArtist = new Artist { Id = 1, Name = ArtistName };
            var secondArtist = new Artist { Id = 2, Name = ArtistName };
            var thirdArtist = new Artist { Id = 3, Name = ArtistName };
            firstArtist.Songs.Add(firstSong);

            await db.Songs.AddRangeAsync(firstSong, secondSong, thirdSong);
            await db.Artists.AddRangeAsync(firstArtist, secondArtist, thirdArtist);
            await db.SaveChangesAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Data;
using News.Data.Models;
using News.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace News.Tests
{
    public class EndpointTests
    {
        private NewsDbContext Context
        {
            get
            {
                DbContextOptions<NewsDbContext> dbOptions = new DbContextOptionsBuilder<NewsDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new NewsDbContext(dbOptions);
            }
        }

        private IEnumerable<NewsEnt> GetTestData()
        {
            return new List<NewsEnt>()
            {
                new NewsEnt() {Id = 1, Title="TestTitle1", Content="TestContent1", PublishDate=DateTime.ParseExact("01/06/2011","dd/MM/yyyy",CultureInfo.InvariantCulture)},
                new NewsEnt() {Id = 2, Title="TestTitle2", Content="TestContent2", PublishDate=DateTime.ParseExact("02/06/2012","dd/MM/yyyy",CultureInfo.InvariantCulture)},
                new NewsEnt() {Id = 3, Title="TestTitle3", Content="TestContent3", PublishDate=DateTime.ParseExact("03/06/2013","dd/MM/yyyy",CultureInfo.InvariantCulture)},
                new NewsEnt() {Id = 4, Title="TestTitle4", Content="TestContent4", PublishDate=DateTime.ParseExact("04/06/2014","dd/MM/yyyy",CultureInfo.InvariantCulture)},
            };
        }

        private void PopulateData(NewsDbContext context)
        {
            context.AddRange(this.GetTestData());
            context.SaveChanges();
        }

        private bool CompareNewsExact(NewsEnt thisNews, NewsEnt otherNews)
        {
            return thisNews.Id == otherNews.Id
                && thisNews.Title == otherNews.Title
                && thisNews.Content == otherNews.Content
                && thisNews.PublishDate == otherNews.PublishDate;
        }

        [Fact]
        public void NewsControllerGetAllNewsShould_ReturnOkStatusCode()
        {
            //Arrenge
            var db = this.Context;

            this.PopulateData(db);
            var newsController = new NewsController(db);

            //Act
            var news = newsController.GetAllNews();

            //Assert
            Assert.IsType<OkObjectResult>(news);
        }

        [Fact]
        public void NewsControllerGetAllNewsShould_ReturnCorrectData()
        {
            //Arrenge
            var db = this.Context;

            this.PopulateData(db);
            var newsController = new NewsController(db);

            //Act
            var news = newsController.GetAllNews();

            //Assert
            var returnedData = (newsController.GetAllNews() as OkObjectResult).Value as IEnumerable<NewsEnt>;
            var testData = this.GetTestData();

            foreach (var returnedModel in returnedData)
            {
                var testModel = testData.First(n => n.Id == returnedModel.Id);

                Assert.NotNull(testModel);
                Assert.True(this.CompareNewsExact(returnedModel, testModel));
            }
        }

        [Fact]
        public void NewsControllerPostNews_ReturnCreatedStatusCode()
        {
            //Arrenge
            var db = this.Context;
            var newsController = new NewsController(db);

            var testModel = this.GetTestData().First();
            //Act
            var news = newsController.Post(testModel);

            //Assert
            Assert.IsType<CreatedAtActionResult>(news);
        }

        [Fact]
        public void NewsControllerPostNews_ReturnCreatedNews()
        {
            //Arrenge
            var db = this.Context;
            var newsController = new NewsController(db);

            var testModel = this.GetTestData().First();
            //Act
            var returnedModel = (newsController.Post(testModel) as CreatedAtActionResult).Value as NewsEnt;

            //Assert
            Assert.True(this.CompareNewsExact(returnedModel, testModel));
        }

        [Fact]
        public void NewsControllerPostNewsWithIncorectData_ReturnBadRequestStatusCode()
        {
            //Arrenge
            var db = this.Context;
            var newsController = new NewsController(db);

            //Act
            var testModel = this.GetTestData().First();

            newsController.ModelState.AddModelError("Invalid Data", "Invalid Data");

            //Assert
            Assert.IsType<BadRequestObjectResult>(newsController.Post(testModel));
        }

        [Fact]
        public void NewsControllerPutNewsWithCorrectData_ReturnOkStatusCode()
        {
            //Arrenge
            var db = this.Context;
            //this.PopulateData(db);
            var newsController = new NewsController(db);

            //Act
            var testModel = this.GetTestData().First();
            var news = newsController.Put(testModel.Id, testModel);

            //Assert
            Assert.IsType<OkResult>(news);
        }

        [Fact]
        public void NewsControllerPutNewsWithCorrectData_ReturnCorrectData()
        {
            //Arrenge
            var db = this.Context;
            this.PopulateData(db);
            var newsController = new NewsController(db);

            //Act
            var testModel = this.GetTestData().First();
            testModel.Title = "Updated";
            testModel.Content = "Updated";
            var news = newsController.Put(testModel.Id, testModel);
            var updatedModel = this.GetTestData().First();

            //Assert
            Assert.True(testModel.Id == updatedModel.Id && testModel.Title != updatedModel.Title && testModel.Content != updatedModel.Content);
        }

        [Fact]
        public void NewsControllerPutNewsWithIncorectData_ReturnBadRequestStatusCode()
        {
            //Arrenge
            var db = this.Context;
            this.PopulateData(db);
            var newsController = new NewsController(db);

            //Act
            var testModel = this.GetTestData().First();

            newsController.ModelState.AddModelError("Invalid Data", "Invalid Data");

            //Assert
            Assert.IsType<NotFoundResult>(newsController.Put(testModel.Id,testModel));
        }

        [Fact]
        public void NewsControllerDeleteExistingNewsData_ReturnedOkStatusCode()
        {
            //Arrenge
            var db = this.Context;
            this.PopulateData(db);
            var newsController = new NewsController(db);

            //Act
            var testModel = this.GetTestData().First();
            var testCode = newsController.Delete(1);

            Assert.IsType<OkResult>(testCode);
        }

        [Fact]
        public void NewsControllerDeleteNonExistingNewsData_ReturnedBadStatusCode()
        {
            //Arrenge
            var db = this.Context;
            var newsController = new NewsController(db);

            //Act
            var testDataCount = this.GetTestData().Count();
            var testCode = newsController.Delete(testDataCount+1);

            Assert.IsType<BadRequestResult>(testCode);
        }
    }
}

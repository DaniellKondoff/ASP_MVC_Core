namespace LearningSystem.Test.Services
{
    using Data;
    using FluentAssertions;
    using LearningSystem.Data.Models;
    using LearningSystem.Services.Implementations;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class CourseServiceTest
    {
        public CourseServiceTest()
        {
            TestStartUp.Initialize();
        }

        [Fact]
        public async Task FindAsyncShouldReturnCorrectResultWithFIlterAndOrder()
        {

            //Arrenge
            var db = this.GetDatabase();

            var firstCourse = new Course { Id = 1, Name = "First" };
            var secondCourse = new Course { Id = 2, Name = "Second" };
            var thirdCourse = new Course { Id = 3, Name = "Third" };

            db.AddRange(firstCourse, secondCourse, thirdCourse);

            await db.SaveChangesAsync();

            var courseService = new CourseService(db);

            //Act
            var result = await courseService.FindAsync("t");

            //Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 3 && r.ElementAt(1).Id == 1)
                .And
                .HaveCount(2);
        }

        [Fact]
        public async Task SignInStudentAsyncShouldSaveCorrectDataWIthValidCOurseIdAndStudentId()
        {
            //Arrenge
            var db = this.GetDatabase();

            const string StudentID = "TestStudentId";
            const int CourseId = 1;
            var course = new Course
            {
                Id = CourseId,
                StartDate = DateTime.MaxValue,
                Students = new List<StudentCourse>()
                
            };

            db.Add(course);

            await db.SaveChangesAsync();

            var courseService = new CourseService(db);

            //Act
            var result = await courseService.SignInStudentAsync(CourseId, StudentID);
            var savedEntry = db.Find<StudentCourse>(StudentID, CourseId);

            //Assert
            result
                .Should()
                .Be(true);

            savedEntry
                .Should()
                .NotBeNull();
        }

        private LearningSystemDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<LearningSystemDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            return new LearningSystemDbContext(dbOptions);
        }
    }
}

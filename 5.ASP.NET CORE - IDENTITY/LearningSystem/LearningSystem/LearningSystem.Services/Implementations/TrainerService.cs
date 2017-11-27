using AutoMapper.QueryableExtensions;
using LearningSystem.Data;
using LearningSystem.Services.Contracts;
using LearningSystem.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSystem.Data.Models;

namespace LearningSystem.Services.Implementations
{
    public class TrainerService : ITrainerService
    {
        private readonly LearningSystemDbContext db;

        public TrainerService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddGrade(int courseId, string studentId, Grade grade)
        {
            var studentInCourse = await this.db
                .FindAsync<StudentCourse>(studentId, courseId);

            if (studentInCourse == null)
            {
                return false;
            }

            studentInCourse.Grade = grade;

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> CoursesByIdAsync(string trainerId)
        {
            return await this.db
                .Courses
                .Where(c => c.TrainerId == trainerId)
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();
        }

        public async Task<bool> IsTrainerCourse(int courseId, string trainerId)
        {
            return await this.db.Courses
                .AnyAsync(c => c.Id == courseId && c.TrainerId == trainerId);
        }

        public async Task<IEnumerable<StudentsInCourseServiceModel>> StudentsInCourseAsync(int courseId)
        {
            return await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.Students.Select(s => s.Student))
                .ProjectTo<StudentsInCourseServiceModel>(new { courseId = courseId })
                .ToListAsync();
        }
    }
}

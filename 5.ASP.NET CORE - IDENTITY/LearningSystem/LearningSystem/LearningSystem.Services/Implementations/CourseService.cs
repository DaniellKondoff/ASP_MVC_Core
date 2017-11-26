using LearningSystem.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using LearningSystem.Services.Models;
using System.Threading.Tasks;
using LearningSystem.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using LearningSystem.Data.Models;

namespace LearningSystem.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly LearningSystemDbContext db;

        public CourseService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> AllActiveAsync()
        {
            return await this.db
                .Courses
                .OrderByDescending(c => c.Id)
                .Where(c => c.StartDate >= DateTime.UtcNow)
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();
        }

        public async Task<CourseDetailsServiceModel> ByIdAsync(int id)
        {
            return await this.db
                .Courses
                .Where(c => c.Id == id)
                .ProjectTo<CourseDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SignInStudentAsync(int courseId, string studentId)
        {
            var courseInfo = await this.GetCourseWithStudentsInfo(courseId, studentId);

            if (courseInfo.StartDate < DateTime.UtcNow || courseInfo.UserIsEnrolledInCourse || courseInfo == null)
            {
                return false;
            }

            var studentInCourse = new StudentCourse
            {
                CourseId = courseId,
                StudentId = studentId
            };

            this.db.Add(studentInCourse);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SignOutStudentAsync(int courseId, string studentId)
        {
            var courseInfo = await this.GetCourseWithStudentsInfo(courseId, studentId);

            if (courseInfo.StartDate < DateTime.UtcNow || !courseInfo.UserIsEnrolledInCourse || courseInfo == null)
            {
                return false;
            }

            var studentInCourse = await this.db.FindAsync<StudentCourse>(studentId, courseId);

            this.db.Remove(studentInCourse);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> StudentIsSignedInCourseAsync(int courseId, string studentId)
        {
            return await this.db
                .Courses
                .AnyAsync(c => c.Id == courseId && c.Students.Any(s => s.StudentId == studentId));
        }

        private async Task<CourseWithStudentsInfo> GetCourseWithStudentsInfo(int courseId, string studentId)
        {
          return  await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .Select(c => new CourseWithStudentsInfo
                {
                    StartDate = c.StartDate,
                    UserIsEnrolledInCourse = c.Students.Any(s => s.StudentId == studentId)
                })
                .FirstOrDefaultAsync();
        }
    }
}

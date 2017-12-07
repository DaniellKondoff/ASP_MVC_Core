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

        public async Task<TModel> ByIdAsync<TModel>(int id) where TModel : class
        {
            return await this.db
                .Courses
                .Where(c => c.Id == id)
                .ProjectTo<TModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CourseListingServiceModel>> FindAsync(string searchText)
        {
            searchText = searchText ?? string.Empty;

            return await this.db
                        .Courses
                        .OrderByDescending(c => c.Id)
                        .Where(c => c.Name.ToLower().Contains(searchText.ToLower()))
                        .ProjectTo<CourseListingServiceModel>()
                        .ToListAsync();
        }

        public async Task<bool> SaveExamSubmission(int courseId, string studentId, byte[] examSubmission)
        {
            var studentInCourse = await this.db.FindAsync<StudentCourse>(studentId, courseId);

            if (studentInCourse == null)
            {
                return false;
            }

            studentInCourse.ExamSubmission = examSubmission;
            await this.db.SaveChangesAsync();

            return true;
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

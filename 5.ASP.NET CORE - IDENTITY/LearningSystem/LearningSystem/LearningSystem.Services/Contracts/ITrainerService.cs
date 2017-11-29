using LearningSystem.Data.Models;
using LearningSystem.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services.Contracts
{
    public interface ITrainerService
    {
        Task<IEnumerable<CourseListingServiceModel>> CoursesByIdAsync(string trainerId);

        Task<IEnumerable<StudentsInCourseServiceModel>> StudentsInCourseAsync(int courseId);

        Task<bool> IsTrainerCourse(int courseId, string trainerId);

        Task<bool> AddGrade(int courseId, string studentId, Grade grade);

        Task<byte[]> GetExamSubmission(int courseId, string studentId);
    }
}

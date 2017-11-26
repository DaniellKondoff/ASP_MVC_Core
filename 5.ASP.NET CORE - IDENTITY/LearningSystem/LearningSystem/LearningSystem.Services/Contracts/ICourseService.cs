using LearningSystem.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services.Contracts
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseListingServiceModel>> AllActiveAsync();

        Task<CourseDetailsServiceModel> ByIdAsync(int id);

        Task<bool> StudentIsSignedInCourseAsync(int courseId, string userId);

        Task<bool> SignInStudentAsync(int courseId, string userId);

        Task<bool> SignOutStudentAsync(int courseId, string studentId);
    }
}

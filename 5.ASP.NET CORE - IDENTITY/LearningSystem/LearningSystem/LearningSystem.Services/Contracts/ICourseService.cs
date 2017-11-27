using LearningSystem.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningSystem.Services.Contracts
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseListingServiceModel>> AllActiveAsync();

        Task<TModel> ByIdAsync<TModel>(int id) where TModel : class;
    
        Task<bool> StudentIsSignedInCourseAsync(int courseId, string userId);

        Task<bool> SignInStudentAsync(int courseId, string userId);

        Task<bool> SignOutStudentAsync(int courseId, string studentId);
    }
}

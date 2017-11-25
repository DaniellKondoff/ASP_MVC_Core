using System;
using System.Threading.Tasks;

namespace LearningSystem.Services.Admin.Contracts
{
    public interface IAdminCourseService
    {
        Task CreateAsync(string name, string description, DateTime startDate, DateTime endDate, string trainerId);
    }
}

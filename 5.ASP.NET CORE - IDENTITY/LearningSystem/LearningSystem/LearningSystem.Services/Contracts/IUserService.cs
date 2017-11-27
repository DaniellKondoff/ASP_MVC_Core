using LearningSystem.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services.Contracts
{
    public interface IUserService
    {
        Task<UserProfileServiceModel> ProfileAsync(string id);
    }
}

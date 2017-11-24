using LearningSystem.Core.Mapping;
using LearningSystem.Data.Models;

namespace LearningSystem.Services.Admin.Models
{
    public class AdminUserListingServiceModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}

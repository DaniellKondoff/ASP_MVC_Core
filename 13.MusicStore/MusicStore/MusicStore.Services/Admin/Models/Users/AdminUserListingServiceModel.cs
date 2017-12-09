using AutoMapper;
using MusicStore.Core.Mapping;
using MusicStore.Data.Models;

namespace MusicStore.Services.Admin.Models.Users
{
    public class AdminUserListingServiceModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }     
    }
}

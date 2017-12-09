using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStore.Services.Admin.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Web.Areas.Admin.Models.Users
{
    public class AdminUsersListingViewModel
    {
        public IEnumerable<SelectListItem> Roles { get; set; }

        public IEnumerable<AdminUserListingServiceModel> Users { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSystem.Web.Areas.Admin.Models.Users
{
    public class AddUserToRoleFormModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        public string UserId { get; set; }

    }
}

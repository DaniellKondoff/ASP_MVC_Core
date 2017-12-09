using System.ComponentModel.DataAnnotations;

namespace MusicStore.Web.Areas.Admin.Models.Users
{
    public class AddUserToRoleFormModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using static MusicStore.Data.DataConstants;

namespace MusicStore.Data.Models
{
   
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        [MinLength(UserFirstNameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserLastNameMaxLength)]
        [MinLength(UserLastNameMinLength)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Web.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Required]
        public string Name { get; set; }


        public string Username { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

    }
}

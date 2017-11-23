using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MinLength(DataConstants.UserNameMinLegth)]
        [MaxLength(DataConstants.UserNameMaxLegth)]
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public List<StudentCourse> Courses { get; set; } = new List<StudentCourse>();

        public List<Course> Trainings { get; set; } = new List<Course>();

        public List<Article> Articles { get; set; } = new List<Article>();
    }
}

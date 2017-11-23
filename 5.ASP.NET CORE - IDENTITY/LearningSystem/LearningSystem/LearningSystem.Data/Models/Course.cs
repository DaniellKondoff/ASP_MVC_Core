using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningSystem.Data.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.CourseNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(DataConstants.CourseDescriptionMaxLength)]
        public string Description { get; set; }

        public string TrainerId { get; set; }

        public User Trainer { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<StudentCourse> Students { get; set; } = new List<StudentCourse>();
    }
}

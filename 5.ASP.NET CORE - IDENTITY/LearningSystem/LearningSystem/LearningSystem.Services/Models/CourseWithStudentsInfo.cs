using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSystem.Services.Models
{
    public class CourseWithStudentsInfo
    {
        public DateTime StartDate { get; set; }

        public bool UserIsEnrolledInCourse { get; set; }
    }
}

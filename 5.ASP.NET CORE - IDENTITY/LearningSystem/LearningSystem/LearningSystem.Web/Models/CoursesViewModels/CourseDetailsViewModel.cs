using LearningSystem.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSystem.Web.Models.CoursesViewModels
{
    public class CourseDetailsViewModel
    {
        public CourseDetailsServiceModel CourseDetails { get; set; }

        public bool UserisSignedInCourse { get; set; }
    }
}

using LearningSystem.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSystem.Web.Models.TrainersViewModels
{
    public class StudentsInCourseViewModel
    {
        public IEnumerable<StudentsInCourseServiceModel> Students { get; set; }

        public CourseListingServiceModel CourseByStudent { get; set; }

    }
}

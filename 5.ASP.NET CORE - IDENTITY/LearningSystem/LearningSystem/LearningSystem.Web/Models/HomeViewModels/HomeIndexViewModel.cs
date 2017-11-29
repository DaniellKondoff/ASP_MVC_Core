using LearningSystem.Services.Models;
using System.Collections.Generic;

namespace LearningSystem.Web.Models.HomeViewModels
{
    public class HomeIndexViewModel : SearchFormModel
    {
        public IEnumerable<CourseListingServiceModel> Courses { get; set; }
    }
}

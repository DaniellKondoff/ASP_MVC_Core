﻿using LearningSystem.Services.Models;
using System.Collections.Generic;

namespace LearningSystem.Web.Models.HomeViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<CourseListingServiceModel> Courses { get; set; } = new List<CourseListingServiceModel>();

        public IEnumerable<UserListingServiceModel> Users { get; set; } = new List<UserListingServiceModel>();

        public string SearchText { get; set; }
    }
}

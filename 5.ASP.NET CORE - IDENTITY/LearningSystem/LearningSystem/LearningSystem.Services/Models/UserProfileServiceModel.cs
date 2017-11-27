using LearningSystem.Core.Mapping;
using LearningSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;

namespace LearningSystem.Services.Models
{
    public class UserProfileServiceModel : IMapFrom<User>, IHaveCustomMapping
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string UserName { get; set; }

        public IEnumerable<UserProfileCourseServiceModel> Courses { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<User, UserProfileServiceModel>()
                .ForMember(u => u.Courses, cfg => cfg.MapFrom(s => s.Courses.Select(c => c.Course)));
        }
    }
}

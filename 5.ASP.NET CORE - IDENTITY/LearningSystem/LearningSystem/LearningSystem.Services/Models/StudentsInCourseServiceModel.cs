﻿using LearningSystem.Core.Mapping;
using LearningSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;

namespace LearningSystem.Services.Models
{
    public class StudentsInCourseServiceModel : IMapFrom<User>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Grade? Grade { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            int courseId = default(int);
            mapper
                .CreateMap<User, StudentsInCourseServiceModel>()
                .ForMember(s => s.Grade, cfg => cfg.MapFrom(u => u.Courses.Where(c => c.CourseId == courseId).Select(c => c.Grade).FirstOrDefault()));
        }
    }
}

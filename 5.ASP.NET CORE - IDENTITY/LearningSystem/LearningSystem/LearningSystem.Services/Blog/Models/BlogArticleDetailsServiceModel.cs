﻿using AutoMapper;
using LearningSystem.Core.Mapping;
using LearningSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSystem.Services.Blog.Models
{
    public class BlogArticleDetailsServiceModel : IMapFrom<Article>, IHaveCustomMapping
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Article, BlogArticleDetailsServiceModel>()
                .ForMember(a => a.Author, cfg => cfg.MapFrom(a => a.Author.UserName));
        }
    }
}

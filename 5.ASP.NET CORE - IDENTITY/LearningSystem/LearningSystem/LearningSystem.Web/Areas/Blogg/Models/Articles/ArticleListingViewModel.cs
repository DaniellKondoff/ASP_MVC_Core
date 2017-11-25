using LearningSystem.Services;
using LearningSystem.Services.Blog.Models;
using System;
using System.Collections.Generic;

namespace LearningSystem.Web.Areas.Blogg.Models.Articles
{
    public class ArticleListingViewModel
    {
        public IEnumerable<BlogArticleListingModel> Artices { get; set; }

        public int TotalArticles { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalArticles / ServiceConstants.BlogArticlesListingPageSize);

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages? this.TotalPages : this.CurrentPage + 1;
    }   
}

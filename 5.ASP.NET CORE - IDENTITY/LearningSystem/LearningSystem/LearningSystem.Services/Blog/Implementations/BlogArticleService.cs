using LearningSystem.Data;
using LearningSystem.Data.Models;
using LearningSystem.Services.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LearningSystem.Services.Blog.Models;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using static LearningSystem.Services.ServiceConstants;
namespace LearningSystem.Services.Blog.Implementations
{
    public class BlogArticleService : IBlogArticleService
    {
        private readonly LearningSystemDbContext db;

        public BlogArticleService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<BlogArticleListingModel>> AllAsync(int page = 1)
        {
            return await this.db
                .Articles
                .OrderByDescending(a => a.PublishDate)
                .Skip((page - 1) * BlogArticlesListingPageSize)
                .Take(BlogArticlesListingPageSize)
                .ProjectTo<BlogArticleListingModel>()
                .ToListAsync();
        }

        public async Task<BlogArticleDetailsServiceModel> ByIdAsync(int id)
        {
           return await this.db
                .Articles
                .Where(a => a.Id == id)
                .ProjectTo<BlogArticleDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(string title, string content, string authorId)
        {
            var article = new Article
            {
                Title = title,
                Content = content,
                AuthorId = authorId,
                PublishDate = DateTime.UtcNow
            };

            this.db.Articles.Add(article);
           await this.db.SaveChangesAsync();
        }

        public async Task<int> TotalAsync()
        {
            return await this.db.Articles.CountAsync();
        }
    }
}

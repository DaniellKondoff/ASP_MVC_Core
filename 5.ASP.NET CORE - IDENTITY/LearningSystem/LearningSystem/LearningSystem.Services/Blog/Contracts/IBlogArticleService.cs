using LearningSystem.Services.Blog.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services.Blog.Contracts
{
    public interface IBlogArticleService
    {
        Task CreateAsync(string title, string content, string authorId);

        Task<IEnumerable<BlogArticleListingModel>> AllAsync(int page = 1);

        Task<BlogArticleDetailsServiceModel> ByIdAsync(int id);

        Task<int> TotalAsync();
    }
}

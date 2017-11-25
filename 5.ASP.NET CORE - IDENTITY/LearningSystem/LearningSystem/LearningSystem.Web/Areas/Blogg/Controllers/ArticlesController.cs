using Ganss.XSS;
using LearningSystem.Data.Models;
using LearningSystem.Services.Blog.Contracts;
using LearningSystem.Services.Html;
using LearningSystem.Web.Areas.Blogg.Models.Articles;
using LearningSystem.Web.Common;
using LearningSystem.Web.Infrastructure.Extensions;
using LearningSystem.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSystem.Web.Areas.Blogg.Controllers
{
    [Area(WebConstants.BlogArea)]
    [Authorize(Roles = WebConstants.BlogAuthorRole)]
    public class ArticlesController : Controller
    {
        private readonly IHtmlService htmlService;
        private readonly IBlogArticleService articleService;
        private readonly UserManager<User> userManager;

        public ArticlesController(IHtmlService htmlService, IBlogArticleService articleService, UserManager<User> userManager)
        {
            this.htmlService = htmlService;
            this.articleService = articleService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            var allArticles = await this.articleService.AllAsync(page);

            return View(new ArticleListingViewModel
            {
                Artices = allArticles,
                TotalArticles = await this.articleService.TotalAsync(),
                CurrentPage = page
            });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var article = await this.articleService.ByIdAsync(id);

            return this.ViewOrNotFounr(article);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Create(PublishArticleFormModel model)
        {
            model.Content = this.htmlService.Sanitize(model.Content);

            var userId = this.userManager.GetUserId(User);

            await this.articleService.CreateAsync(model.Title, model.Content, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}

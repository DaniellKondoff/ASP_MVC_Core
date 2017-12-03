using BookShop.Api.Infrastructure.Extensions;
using BookShop.Api.Infrastructure.Filters;
using BookShop.Api.Models.Authors;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BookShop.Api.WebConstants;

namespace BookShop.Api.Controllers
{
    public class AuthorsController : BaseController
    {
        private readonly IAuthorService authorService;

        public AuthorsController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpGet(WithId)]
        public IActionResult Get(int id)
        {
            return this.OkOrNotFound(this.authorService.Details(id));
        }

        [HttpGet(WithId + "/books")]
        public async  Task<IActionResult> GetBooks(int id)
        {
            return this.Ok(await this.authorService.Books(id));
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Post([FromBody]PostAuthorRequestModel model)
        {
            var id = await this.authorService.Create(model.FirstName, model.LastName);

            return Ok(id);
        }
    }
}

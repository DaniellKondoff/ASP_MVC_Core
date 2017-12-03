using BookShop.Api.Infrastructure.Extensions;
using BookShop.Api.Infrastructure.Filters;
using BookShop.Api.Models.Books;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BookShop.Api.WebConstants;

namespace BookShop.Api.Controllers
{
    public class BooksController : BaseController
    {
        private readonly IBookService bookService;
        private readonly IAuthorService authorService;

        public BooksController(IBookService bookService, IAuthorService authorService)
        {
            this.bookService = bookService;
            this.authorService = authorService;
        }

        [HttpGet(WithId)]
        public async Task<IActionResult> Get(int id)
        {
            return this.OkOrNotFound(await this.bookService.Details(id));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string search)
        {
            return this.OkOrNotFound(await this.bookService.All(search));
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Post([FromBody] CreateBookRequestModel model)
        {
            if (!await this.authorService.Exist(model.AuthorId))
            {
                return BadRequest("Author does not exist.");
            }

            var bookId = await this.bookService.Create(
                model.Title,
                model.Description,
                model.Price,
                model.Copies,
                model.Edition,
                model.AgeRestriction,
                model.ReleaseDate,
                model.AuthorId,
                model.Categories);

            return Ok(bookId);
        }

        [HttpPut(WithId)]
        [ValidateModelState]
        public async Task<IActionResult> Put(int id, [FromBody] EditBookRequestModel model)
        {
            if (!await this.authorService.Exist(model.AuthorId))
            {
                return BadRequest("Author does not exist.");
            }


            var success = await this.bookService.Edit(
                id,
                model.Title,
                model.Description,
                model.Price,
                model.Copies,
                model.Edition,
                model.AgeRestriction,
                model.ReleaseDate,
                model.AuthorId);

            if (!success)
            {
                return BadRequest();
            }

            return Ok(id);
        }

        [HttpDelete(WithId)]
        public async Task<IActionResult> Delete(int id)
        {
            var isExists = await this.bookService.Exists(id);

            if (!isExists)
            {
                return NotFound();
            }

            var success = await this.bookService.Delete(id);

            if (!success)
            {
                BadRequest();
            }

            return Ok(id);
        }
    }
}

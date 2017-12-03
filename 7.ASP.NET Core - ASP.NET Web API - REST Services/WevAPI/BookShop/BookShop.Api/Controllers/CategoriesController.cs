using BookShop.Api.Infrastructure.Filters;
using BookShop.Api.Models.Categories;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BookShop.Api.WebConstants;

namespace BookShop.Api.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.categoryService.All());
        }

        [HttpGet(WithId)]
        public async Task<IActionResult> Get(int id)
        {
            var category = await this.categoryService.FindBy(id);
            if (category == null)
            {
                return NotFound();
            }

            return this.Ok(category);
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Post([FromBody] CategoryRequestModel model)
        {
            var categoryExist = await this.categoryService.Exist(model.Name);

            if (categoryExist)
            {
                return BadRequest("That Category already exists");
            }

            var categoryId = this.categoryService.Create(model.Name);

            return Ok(categoryId);
        }

        [HttpPut(WithId)]
        [ValidateModelState]
        public async Task<IActionResult> Edit(int id, [FromBody] CategoryRequestModel model)
        {
            var categoryExist = await this.categoryService.Exist(model.Name);

            if (categoryExist)
            {
                return BadRequest("That Category already exists");
            }

            var success = this.categoryService.Edit(id, model.Name);

            return Ok(id);
        }

        [HttpDelete(WithId)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await this.categoryService.Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            return Ok(id);
        }
    }
}

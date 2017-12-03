using AutoMapper.QueryableExtensions;
using BookShop.Data;
using BookShop.Data.Models;
using BookShop.Services.Models.Categories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly BookShopDbContext db;

        public CategoryService(BookShopDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<CategoryListingServiceModel>> All()
        {
            return await this.db.Categories
                .ProjectTo<CategoryListingServiceModel>()
                .ToListAsync();
        }

        public async Task<bool> Exist(string name)
        {
            using (db)
            {
                return await this.db.Categories.AnyAsync(c => c.Name.ToLower().Equals(name.ToLower()));
            }
        }

        public async Task<CategoryListingServiceModel> FindBy(int id)
        {
            return await this.db.Categories
                .Where(c => c.Id == id)
                .ProjectTo<CategoryListingServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> Create(string name)
        {
            var category = new Category
            {
                Name = name
            };

            this.db.Categories.Add(category);
            await this.db.SaveChangesAsync();

            return category.Id;
        }

        public async Task<bool> Edit(int id, string name)
        {
            var category = this.db
                .Categories
                .Where(c => c.Id == id)
                .FirstOrDefault();

            if (category == null)
            {
                return false;
            }

            category.Name = name;

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var category = await this.db.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return false;
            }

            this.db.Categories.Remove(category);
            await this.db.SaveChangesAsync();

            return true;
        }
    }
}

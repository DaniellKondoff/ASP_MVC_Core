using AutoMapper.QueryableExtensions;
using BookShop.Data;
using BookShop.Services.Models.Books;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using BookShop.Data.Models;
using BookShop.Common.Extensions;

namespace BookShop.Services
{
    public class BookService : IBookService
    {
        private readonly BookShopDbContext db;

        public BookService(BookShopDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<BookListingServiceModel>> All(string searchTect)
        {
            return await this.db.Books
                .Where(b => b.Title.ToLower().Contains(searchTect.ToLower()))
                .OrderBy(b => b.Title)
                .Take(10)
                .ProjectTo<BookListingServiceModel>()
                .ToListAsync();
        }

        public async Task<int> Create(
            string title, 
            string desciprion, 
            decimal price, 
            int copies, 
            int? edition, 
            int? ageResitriction, 
            DateTime realeaseDate, 
            int AuthorId, 
            string Categories)
        {
            var categoryNames = Categories
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToHashSet();

            var existingCategories = await this.db.Categories
                .Where(c => categoryNames.Contains(c.Name))
                .ToListAsync();

            var allCategories = new List<Category>(existingCategories);
            foreach (var categoryName in categoryNames)
            {
                if (existingCategories.All(c => c.Name != categoryName))
                {
                    var category = new Category
                    {
                        Name = categoryName
                    };

                    this.db.Add(category);

                    allCategories.Add(category);
                }
            }

            await this.db.SaveChangesAsync();

            var book = new Book
            {
                Title = title,
                Description = desciprion,
                Price = price,
                Copies = copies,
                Edition = edition,
                AgeRestriction = ageResitriction,
                ReleaseDate = realeaseDate,
                AuthorId = AuthorId
            };

            existingCategories.ForEach(c => book.Categories.Add(new BookCategory
            {
                CategoryId = c.Id
            }));

            this.db.Books.Add(book);
            await this.db.SaveChangesAsync();

            return book.Id;
        }

        public async Task<bool> Delete(int id)
        {
            var book = await this.db.Books
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return false;
            }

            this.db.Books.Remove(book);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<BookDetailsServiceModel> Details(int id)
        {
            return await this.db.Books
                .Where(b => b.Id == id)
                .ProjectTo<BookDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Edit(int id, string title, string desciprion, decimal price, int copies, int? edition, int? ageResitriction, DateTime realeaseDate, int AuthorId)
        {
            var book = await this.db.Books.FindAsync(id);

            if (book==null)
            {
                return false;
            }

            book.Title = title;
            book.Description = desciprion;
            book.Price = price;
            book.Copies = copies;
            book.Edition = edition;
            book.AgeRestriction = ageResitriction;
            book.ReleaseDate = realeaseDate;
            book.AuthorId = AuthorId;

            await this.db.SaveChangesAsync();

            return true;
        }

        public Task<bool> Exists(int id)
        {
            return this.db.Books.AnyAsync(b => b.Id == id);
        }
    }
}

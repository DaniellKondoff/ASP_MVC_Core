using System;
using System.Collections.Generic;
using System.Text;
using BookShop.Services.Models.Authors;
using BookShop.Data;
using System.Linq;
using AutoMapper.QueryableExtensions;
using BookShop.Data.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly BookShopDbContext db;

        public AuthorService(BookShopDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<BookByAuthorServiceModel>> Books(int authorId)
        {
            return await this.db.Books
                .Where(b => b.AuthorId == authorId)
                .ProjectTo<BookByAuthorServiceModel>()
                .ToListAsync();
        }

        public async Task<int> Create(string firstName, string lastName)
        {
            var author = new Author
            {
                FirstName = firstName,
                LastName = lastName
            };

            this.db.Authors.Add(author);
            await this.db.SaveChangesAsync();

            return author.Id;
        }

        public AuthorDetailsServiceModel Details(int id)
        {
           return this.db.Authors
                .Where(a => a.Id == id)
                .ProjectTo<AuthorDetailsServiceModel>()
                .FirstOrDefault();
        }

        public async Task<bool> Exist(int authorId)
        {
            return await this.db.Authors
                .AnyAsync(a => a.Id == authorId);
        }
    }
}

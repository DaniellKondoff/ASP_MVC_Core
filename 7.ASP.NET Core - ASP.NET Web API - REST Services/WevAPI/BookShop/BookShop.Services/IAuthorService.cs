using BookShop.Services.Models.Authors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Services
{
    public interface IAuthorService
    {
        AuthorDetailsServiceModel Details(int id);

        Task<int> Create(string firstName, string lastName);

        Task<IEnumerable<BookByAuthorServiceModel>> Books(int authorId);

        Task<bool> Exist(int authorId);
    }
}

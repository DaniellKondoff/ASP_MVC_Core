using BookShop.Services.Models.Books;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Services
{
    public interface IBookService
    {
        Task<BookDetailsServiceModel> Details(int id);

        Task<bool> Exists(int id);

        Task<bool> Delete(int id);

        Task<IEnumerable<BookListingServiceModel>> All(string searchText);

        Task<int> Create(
            string title, 
            string desciprion, 
            decimal price, 
            int copies, 
            int? edition, 
            int? ageResitriction, 
            DateTime realeaseDate, 
            int AuthorId, 
            string Categories);

        Task<bool> Edit(
            int id,
            string title,
            string desciprion,
            decimal price,
            int copies,
            int? edition,
            int? ageResitriction,
            DateTime realeaseDate,
            int AuthorId);
    }
}

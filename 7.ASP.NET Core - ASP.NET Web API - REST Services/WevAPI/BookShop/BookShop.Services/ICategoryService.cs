using BookShop.Services.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryListingServiceModel>> All();

        Task<CategoryListingServiceModel> FindBy(int id);

        Task<bool> Exist(string name);

        Task<int> Create(string name);

        Task<bool> Edit(int id, string name);

        Task<bool> Delete(int id);

    }
}

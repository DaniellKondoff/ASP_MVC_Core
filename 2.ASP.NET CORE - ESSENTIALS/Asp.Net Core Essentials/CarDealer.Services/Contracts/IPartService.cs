using CarDealer.Services.Models.Parts;
using System.Collections.Generic;

namespace CarDealer.Services.Contracts
{
    public interface IPartService
    {
        IEnumerable<PartListingModel> AllListings(int page = 1, int pageSize = 10);

        IEnumerable<PartsShortModel> All();

        int Total();

        void Create(string name, decimal price, double quantity, int supplierId);

        void Delete(int id);

        PartEditModel GetById(int id);

        void Edit(int id, decimal price, double quantity);
    }
}

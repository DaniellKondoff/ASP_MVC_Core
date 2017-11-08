using CarDealer.Services.Models.Parts;
using System.Collections.Generic;

namespace CarDealer.Services.Contracts
{
    public interface IPartService
    {
        IEnumerable<PartListingModel> All(int page = 1, int pageSize = 10);

        int Total();
        void Create(string name, decimal price, double quantity, int supplierId);
    }
}

using CarDealer.Services.Models.Parts;
using CarDealer.Services.Models.Suppliers;
using System.Collections.Generic;

namespace CarDealer.Services.Contracts
{
    public interface ISupplierService
    {
        IEnumerable<SupplierListingModel> AllListing(bool isImporter);

        IEnumerable<SupplierModel> All();

        IEnumerable<AllSupplierModel> AllBoth();

        void Create(string name, bool isImporter, IEnumerable<int> selectedParts);

        SupplierEditModel GetById(int id);

        void Edit(int id, string name, bool isImporter, IEnumerable<int> partIds);

        void Delete(int id);
    }
}

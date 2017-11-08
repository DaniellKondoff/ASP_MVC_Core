using CarDealer.Services.Models.Suppliers;
using System.Collections.Generic;

namespace CarDealer.Services.Contracts
{
    public interface ISupplierService
    {
        IEnumerable<SupplierListingModel> AllListing(bool isImporter);

        IEnumerable<SupplierModel> All();
    }
}

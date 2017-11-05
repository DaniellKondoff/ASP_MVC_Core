using CarDealer.Services.Models;
using System.Collections.Generic;

namespace CarDealer.Services.Contracts
{
    public interface ISupplierService
    {
        IEnumerable<SupplierModel> All(bool isImporter);
    }
}

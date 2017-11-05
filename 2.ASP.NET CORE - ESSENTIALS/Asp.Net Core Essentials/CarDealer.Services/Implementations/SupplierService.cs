using CarDealer.Data;
using CarDealer.Services.Contracts;
using CarDealer.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarDealer.Services.Implementations
{
    public class SupplierService : ISupplierService
    {
        private readonly CarDealerDbContext db;

        public SupplierService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SupplierModel> All(bool IsImporter)
        {
            return this.db
                .Suppliers
                .Where(s => s.IsImporter == IsImporter)
                .Select(s => new SupplierModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    TotalParts = s.Parts.Count
                })
                .ToList();
        }
    }
}

using CarDealer.Data;
using CarDealer.Data.Models;
using CarDealer.Services.Contracts;
using CarDealer.Services.Models.Parts;
using CarDealer.Services.Models.Suppliers;
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

        public IEnumerable<SupplierModel> All()
        {
            return this.db
                .Suppliers
                .OrderBy(s => s.Name)
                .Select(s => new SupplierModel
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();
        }

        public IEnumerable<AllSupplierModel> AllBoth()
        {
            return this.db
                .Suppliers
                .OrderBy(s => s.Name)
                .Select(s => new AllSupplierModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsImporter = s.IsImporter
                })
                .ToList();
        }

        public IEnumerable<SupplierListingModel> AllListing(bool IsImporter)
        {
            return this.db
                .Suppliers
                .Where(s => s.IsImporter == IsImporter)
                .Select(s => new SupplierListingModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    TotalParts = s.Parts.Count
                })
                .ToList();
        }

        public void Create(string name, bool isImporter, IEnumerable<int> parts)
        {
            var existingParts = this.db.Parts
                .Where(p => parts.Contains(p.Id))
                .Select(p => p.Id)
                .ToList();

            var supplier = new Supplier
            {
                Name = name,
                IsImporter = isImporter
            };

            foreach (var partId in existingParts)
            {
                var part = this.db.Parts.Find(partId);
                supplier.Parts.Add(part);
            }

            db.Suppliers.Add(supplier);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var supplier = this.db.Suppliers.Find(id);

            if (supplier == null)
            {
                return;
            }

            this.db.Parts.RemoveRange(supplier.Parts);
            this.db.Suppliers.Remove(supplier);

            this.db.SaveChanges();
        }

        public void Edit(int id, string name, bool isImporter, IEnumerable<int> partIds)
        {
            var supplier = this.db
                .Suppliers.Find(id);


            if (supplier == null)
            {
                return;
            }

            var existingParts = this.db.Parts
                .Where(p => partIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToList();

            supplier.Name = name;
            supplier.IsImporter = isImporter;

            supplier.Parts.Clear();

            foreach (var partId in existingParts)
            {
                var part = this.db.Parts.Find(partId);
                supplier.Parts.Add(part);
            }

            this.db.SaveChanges();
        }

        public SupplierEditModel GetById(int id)
        {
            return this.db.Suppliers
                .Where(s => s.Id == id)
                .Select(s => new SupplierEditModel
                {
                    Name = s.Name,
                    IsImporter = s.IsImporter,
                    AllParts = s.Parts.Select(p=> new PartsShortModel
                    {
                        Id = p.Id,
                        Name = p.Name
                    })
                })
                .FirstOrDefault();
        }
    }
}

﻿using CarDealer.Data;
using CarDealer.Data.Models;
using CarDealer.Services.Contracts;
using CarDealer.Services.Models.Parts;
using System.Collections.Generic;
using System.Linq;

namespace CarDealer.Services.Implementations
{
    public class PartService : IPartService
    {

        private readonly CarDealerDbContext db;

        public PartService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<PartListingModel> All(int page = 1, int pageSize=10)
        {
            return this.db
                .Parts
                .OrderByDescending(p=>p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PartListingModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierName = p.Supplier.Name
                })
                .ToList();
        }

        public void Create(string name, decimal price, double quantity, int supplierId)
        {
            var part = new Part
            {
                Name = name,
                Price = price,
                Quantity = quantity > 0 ? quantity : 1,
                SupplierId = supplierId
            };

            db.Parts.Add(part);
            db.SaveChanges();
        }

        public int Total() => this.db.Parts.Count();
       
    }
}
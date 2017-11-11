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

        public IEnumerable<PartsShortModel> All()
        {
            return this.db
                .Parts
                .OrderBy(p=>p.Name)
                .Select(p => new PartsShortModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }

        public IEnumerable<PartListingModel> AllListings(int page = 1, int pageSize=10)
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

        public void Delete(int id)
        {
            var part = db.Parts.Find(id);

            if (part == null)
            {
                return;
            }

            this.db.Parts.Remove(part);
            this.db.SaveChanges();
        }

        public void Edit(int id, decimal price, double quantity)
        {
            var part = this.db.Parts.Find(id);

            if (part == null)
            {
                return;
            }
            part.Price = price;
            part.Quantity = quantity;

            db.SaveChanges();
        }

        public PartEditModel GetById(int id)
        {
            return this.db.Parts
                .Where(p => p.Id == id)
                .Select(p => new PartEditModel
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                })
                .FirstOrDefault();
        }

        public int Total() => this.db.Parts.Count();
       
    }
}

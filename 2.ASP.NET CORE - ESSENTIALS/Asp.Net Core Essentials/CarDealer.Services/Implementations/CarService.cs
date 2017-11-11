using CarDealer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using CarDealer.Services.Models.Cars;
using CarDealer.Data;
using CarDealer.Services.Models.Parts;
using CarDealer.Data.Models;

namespace CarDealer.Services.Implementations
{
    public class CarService : ICarService
    {
        private readonly CarDealerDbContext db;

        public CarService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CarByMakeModel> ByMake(string make)
        {
            return db.Cars
                .Where(c => c.Make.ToLower() == make.ToLower())
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new CarByMakeModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToList();
               
        }

        public void Create(string make, string model, long travelledDistance, IEnumerable<int> parts)
        {
            var existingPartsId = this.db.Parts
                .Where(p => parts.Contains(p.Id))
                .Select(p => p.Id)
                .ToList();

            var car = new Car
            {
                Make = make,
                Model = model,
                TravelledDistance = travelledDistance
            };

            foreach (var partId in existingPartsId)
            {
                car.Parts.Add(new PartCars { PartId = partId });
            }

            this.db.Cars.Add(car);
            this.db.SaveChanges();
        }

        public int Total() => this.db.Cars.Count();

        public IEnumerable<CarWIthPartsModel> WithParts(int page = 1, int pageSize = 10)
        {
            return this.db
                .Cars
                .OrderBy(c=>c.Make)
                .Skip((page -1) * pageSize)
                .Take(pageSize)
                .Select(c => new CarWIthPartsModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.Parts.Select(p => new PartModel
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                })
                .ToList();
        }
    }
}

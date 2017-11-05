using CarDealer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using CarDealer.Services.Models.Cars;
using CarDealer.Data;
using CarDealer.Services.Models.Parts;

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

        public IEnumerable<CarWIthPartsModel> WithParts()
        {
            return this.db
                .Cars
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

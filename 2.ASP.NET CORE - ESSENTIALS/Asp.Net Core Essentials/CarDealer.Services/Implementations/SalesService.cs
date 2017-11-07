using CarDealer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using CarDealer.Services.Models.Sales;
using CarDealer.Data;
using System.Linq;
using CarDealer.Services.Models.Cars;

namespace CarDealer.Services.Implementations
{
    public class SalesService : ISalesService
    {
        private readonly CarDealerDbContext db;

        public SalesService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SaleListModel> All(int? id, bool discounted, double? percentage)
        {
            var salesQuery = db.Sales.AsQueryable();

            if (id != null)
            {
                salesQuery = salesQuery.Where(s => s.Id == id);
            }
            if (discounted)
            {
                salesQuery = salesQuery.Where(s => s.Discount != 0);
            }
            if (percentage != null)
            {
                salesQuery = salesQuery.Where(s => s.Discount == percentage);
            }

            var sales = salesQuery
                .Select(s => new SaleListModel
                {
                    Id = s.Id,
                    Car = new CarByMakeModel
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Customer = s.Customer.Name,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    PriceWithDiscount = s.Car.Parts.Sum(p => p.Part.Price) * (1M - Convert.ToDecimal(s.Discount)),
                    Discount = s.Discount
                })
                .ToList();

            return sales;
        }
    }
}

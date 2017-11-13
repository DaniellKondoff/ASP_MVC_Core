using CarDealer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using CarDealer.Services.Models.Sales;
using CarDealer.Data;
using System.Linq;
using CarDealer.Services.Models.Cars;
using CarDealer.Data.Models;
using Microsoft.EntityFrameworkCore;
using CarDealer.Services.Models.Parts;

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

        public IEnumerable<SaleBasicViewModel> AllBasics()
        {
            return this.db
                .Sales
                .Select(c => new SaleBasicViewModel
                {
                    Id = c.Id,
                    Discount = c.Discount
                })
                .ToList();
        }

        public SaleFinilizeServiceModel ReviewSale(int customerId, int carId, int discountId)
        {
            var customer = this.db.Customers.Find(customerId);
            var car = this.db.Cars.Where(c => c.Id == carId)
                .Select(c => new CarWIthPartsModel
                {
                    Model = c.Model,
                    Make = c.Make,
                    Parts = c.Parts.Select(p => new PartModel
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                })
                .FirstOrDefault();
            
            var discount = this.db.Sales.Find(discountId);

            var sale = new SaleFinilizeServiceModel
            {
                CustomerName = customer.Name,
                CarName = $"{car.Model} {car.Make}",
                Discount = discount.Discount,
                CarPrice = car.Parts.Sum(p=>p.Price)
            };

            sale.FinalCarPrice = sale.CarPrice - (decimal)(sale.Discount);

            return sale;
        }
    }
}

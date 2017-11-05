using CarDealer.Services.Contracts;
using System;
using System.Collections.Generic;
using CarDealer.Services.Models.Customers;
using CarDealer.Data;
using System.Linq;

namespace CarDealer.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly CarDealerDbContext db;

        public CustomerService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CustomerModel> OrderedCustomers(OrderDirection order)
        {
            var customersQuery = this.db.Customers.AsQueryable();

            switch (order)
            {
                case OrderDirection.Ascending:
                    customersQuery = customersQuery
                        .OrderBy(c => c.BirthDate)
                        .ThenBy(c => c.IsYoungDriver);
                    break;
                case OrderDirection.Descending:
                    customersQuery = customersQuery
                        .OrderByDescending(c => c.BirthDate)
                        .ThenBy(c => c.IsYoungDriver);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid Order Direction : {order}.");               
            }

            return customersQuery
               .Select(c => new CustomerModel
               {
                   Name = c.Name,
                   BirthDate = c.BirthDate,
                   IsYoungDriver = c.IsYoungDriver
               })
               .ToList();
        }

        public CustomerWithSales FindWithSales(int id)
        {
            return db.Customers
                .Where(c => c.Id == id)
                .Select(c => new CustomerWithSales
                {
                    Name = c.Name,
                    TotalCars = c.Sales.Count,
                    TotalSpentMoney = c.Sales
                    .Sum(s => s.Car.Parts.Sum(p=>p.Part.Price))
                })
                .FirstOrDefault();
        }
    }
}

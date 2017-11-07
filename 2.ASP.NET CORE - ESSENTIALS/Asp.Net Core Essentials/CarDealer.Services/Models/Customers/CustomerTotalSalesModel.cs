using CarDealer.Services.Models.Cars;
using System.Collections.Generic;
using System.Linq;

namespace CarDealer.Services.Models.Customers
{
    public class CustomerTotalSalesModel
    {
        public string Name { get; set; }

        public bool IsYoungDriver { get; set; }

        public IEnumerable<SalesModel> BoughtCars { get; set; }

        public decimal TotalSpentMoney
                 => this.BoughtCars
                      .Sum(c => c.Price * (1M - (decimal)c.Discount))
                          * (this.IsYoungDriver ? 0.95m : 1);


    }
}

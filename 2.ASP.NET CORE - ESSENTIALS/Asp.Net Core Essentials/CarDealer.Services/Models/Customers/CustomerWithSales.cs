using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Models.Customers
{
    public class CustomerWithSales
    {
        public string Name { get; set; }

        public int TotalCars { get; set; }

        public decimal TotalSpentMoney { get; set; }
    }
}

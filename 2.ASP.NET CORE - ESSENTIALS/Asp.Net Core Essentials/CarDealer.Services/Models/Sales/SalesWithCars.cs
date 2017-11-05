using CarDealer.Services.Models.Cars;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Models.Sales
{
    public class SalesWithCars
    {
        public CarByMakeModel Car { get; set; }

        public string Customer { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithDiscount { get; set; }

        public double Discount { get; set; }
    }
}

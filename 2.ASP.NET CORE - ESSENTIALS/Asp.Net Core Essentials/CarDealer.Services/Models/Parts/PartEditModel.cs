using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Models.Parts
{
    public class PartEditModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }
    }
}

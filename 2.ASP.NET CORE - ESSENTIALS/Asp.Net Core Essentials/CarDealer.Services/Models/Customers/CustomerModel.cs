using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Models.Customers
{
    public class CustomerModel
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}

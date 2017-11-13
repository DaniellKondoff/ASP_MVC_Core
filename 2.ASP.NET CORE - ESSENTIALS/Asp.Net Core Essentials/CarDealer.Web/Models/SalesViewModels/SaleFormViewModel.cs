using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Models.SalesViewModels
{
    public class SaleFormViewModel
    {
        public string CustomerName { get; set; }

        public string CarName { get; set; }

        public double Discount { get; set; }

        public decimal CarPrice { get; set; }

        public decimal FinalCarPrice { get; set; }
    }
}

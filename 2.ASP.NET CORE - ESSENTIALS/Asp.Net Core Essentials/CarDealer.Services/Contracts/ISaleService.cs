using CarDealer.Services.Models.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Contracts
{
    public interface ISaleService
    {
        IEnumerable<SalesWithCars> All(int? id, bool discounted, double? percentage);
    }
}

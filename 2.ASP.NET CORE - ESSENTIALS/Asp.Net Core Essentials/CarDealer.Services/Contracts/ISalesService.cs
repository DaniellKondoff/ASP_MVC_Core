using CarDealer.Services.Models.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Contracts
{
    public interface ISalesService
    {
        IEnumerable<SaleListModel> All(int? id, bool discounted, double? percentage);

        IEnumerable<SaleBasicViewModel> AllBasics();

        SaleFinilizeServiceModel ReviewSale(int customerId, int carId, int discountId);
    }
}

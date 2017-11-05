using CarDealer.Services.Models.Customers;
using System.Collections.Generic;

namespace CarDealer.Services.Contracts
{
    public interface ICustomerService
    {
        IEnumerable<CustomerModel> OrderedCustomers(OrderDirection order);

        CustomerWithSales FindWithSales(int id);
    }
}

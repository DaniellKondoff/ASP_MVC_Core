using CarDealer.Services.Models.Customers;
using System;
using System.Collections.Generic;

namespace CarDealer.Services.Contracts
{
    public interface ICustomerService
    {
        IEnumerable<CustomerModel> OrderedCustomers(OrderDirection order);

        IEnumerable<CustomerBasicModel> AllBasic();

        CustomerTotalSalesModel TotalSalesById(int id);

        void Create(string name, DateTime birthDate, bool isYoungDriver);

        CustomerModel ById(int id);

        void Edit(int id, string name, DateTime birthDate, bool isYoungDriver);

        bool Exists(int id);
    }
}

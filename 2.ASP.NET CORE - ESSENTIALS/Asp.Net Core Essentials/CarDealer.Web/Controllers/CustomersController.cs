using CarDealer.Services.Contracts;
using CarDealer.Services.Models.Customers;
using CarDealer.Web.Models.CustomersModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Controllers
{
    [Route("customers")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [Route("all/{order}")]
        public IActionResult All(string order)
        {
            var orderDirection = order.ToLower() == "descending"
                ? OrderDirection.Descending
                : OrderDirection.Ascending;

            var customers = this.customerService.OrderedCustomers(orderDirection);

            return View(new AllCustomersModel
            {
                Customers = customers,
                OrderDirection = orderDirection
            });
        }

        [Route("{id}")]
        public IActionResult CustomerById(int id)
        {
            return View(this.customerService.FindWithSales(id));
        }

        [Route("all")]
        public IActionResult All()
        {
            var customers = this.customerService.OrderedCustomers(OrderDirection.Ascending);

            return View(new AllCustomersModel
            {
                Customers = customers,
                OrderDirection = OrderDirection.Ascending
            });
        }
    }
}

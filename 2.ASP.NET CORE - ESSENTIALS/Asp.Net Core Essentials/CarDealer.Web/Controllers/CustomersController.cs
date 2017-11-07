using CarDealer.Services.Contracts;
using CarDealer.Services.Models.Customers;
using CarDealer.Web.Models.CustomersModels;
using Microsoft.AspNetCore.Mvc;

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
            return View(this.customerService.TotalSalesById(id));
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

        [Route("add")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Create(CustomerFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.customerService.Create(model.Name, model.BirthDate, model.IsYoungDriver);

            return this.RedirectToAction(nameof(All));
        }

        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id)
        {
            var customer = this.customerService.ById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(new CustomerFormModel
            {
                Name= customer.Name,
                BirthDate = customer.BirthDate,
                IsYoungDriver = customer.IsYoungDriver 
            });
        }

        [Route(nameof(Edit) + "/{id}")]
        [HttpPost]
        public IActionResult Edit(int id, CustomerFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool customerExist = this.customerService.Exists(id);

            if (!customerExist)
            {
                return NotFound();
            }
            
            this.customerService.Edit(id, model.Name, model.BirthDate, model.IsYoungDriver);

            return this.RedirectToAction(nameof(All));
        }
    }
}

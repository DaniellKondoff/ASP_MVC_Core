namespace CarDealer.Web.Controllers
{
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Sales;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using CarDealer.Web.Models.SalesViewModels;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using CarDealer.Data.Models.Enums;
    using System;
    using CarDealer.Web.Infrastructure.Filters;

    [Route("Sales")]
    public class SalesController : Controller
    {
        private const string SaleTable = "Sales";
        private readonly ISalesService saleService;
        private readonly ICustomerService customerService;
        private readonly ICarService carService;

        public SalesController(ISalesService saleService, ICustomerService customerService, ICarService carService)
        {
            this.saleService = saleService;
            this.customerService = customerService;
            this.carService = carService;
        }

        [Route("")]
        public IActionResult All()
        {
            return View(this.GetSalesWithCarsModel(null, false));
        }

        [Route(nameof(Create))]
        [Authorize]
        public IActionResult Create()
        {
            return this.View(new CreateSaleViewModel
            {
                Customers = this.GetAllBasicCustomers(),
                Cars = this.GetAllBasicCars(),
                Discounts = this.GetAllBasicSales()
            });
        }

        [Authorize]
        [Route(nameof(Finalize))]
        [Log(Operation.Add, SaleTable)]
        public IActionResult Finalize(int customerId, int carId, int discountId)
        {
            var saleModel = this.saleService.ReviewSale(customerId, carId, discountId);
            return View(saleModel);
        }

        [HttpPost]
        [Route(nameof(Finalize))]
        [Authorize]
        public IActionResult Finalize(SaleFinilizeServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.saleService.Create(model.CarId,model.CustomerId,model.Discount);
            return RedirectToAction(nameof(All));
        }

        [Route("{id}")]
        public IActionResult Details(int id)
        {
            return this.ViewOrNotFound(this.GetSalesWithCarsModel(id, false));
        }

        [Route("discounted")]
        public IActionResult Discounted()
        {
            return this.View("All", this.GetSalesWithCarsModel(null, true));
        }

        [Route("discounted/{percent}")]
        public IActionResult Discounted(double percent)
        {
            return this.ViewOrNotFound(this.GetSalesWithCarsModel(null, true, percent), "All");
        }

        private IEnumerable<SaleListModel> GetSalesWithCarsModel(int? id, bool discounted)
        {
            return this.saleService.All(id, discounted, null);
        }

        private IEnumerable<SaleListModel> GetSalesWithCarsModel(int? id, bool discounted, double percentage)
        {
            return this.saleService.All(id, discounted, percentage);
        }

        private IEnumerable<SelectListItem> GetAllBasicCustomers()
        {
            return this.customerService.AllBasic()
                 .Select(c => new SelectListItem
                 {
                     Text = c.Name,
                     Value = c.Id.ToString(),
                     Selected = true
                 });
        }

        private IEnumerable<SelectListItem> GetAllBasicCars()
        {
            return this.carService.AllBasic()
                 .Select(c => new SelectListItem
                 {
                     Text = c.Name,
                     Value = c.Id.ToString()
                 });
        }

        private IEnumerable<SelectListItem> GetAllBasicSales()
        {
            return this.saleService.AllBasics()
                 .Select(c => new SelectListItem
                 {
                     Text = c.Discount.ToPercentage(),
                     Value = c.Id.ToString()
                 });
        }
    }
}

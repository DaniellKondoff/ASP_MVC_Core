namespace CarDealer.Web.Controllers
{
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Sales;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Infrastructure.Extensions;

    [Route("Sales")]
    public class SalesController : Controller
    {
        private readonly ISalesService saleService;

        public SalesController(ISalesService saleService)
        {
            this.saleService = saleService;
        }

        [Route("")]
        public IActionResult All()
        {
            return View(this.GetSalesWithCarsModel(null,false));
        }

        [Route("{id}")]
        public IActionResult Details(int id)
        {
            return this.ViewOrNotFound(this.GetSalesWithCarsModel(id, false));
        }

        [Route("discounted")]
        public IActionResult Discounted()
        {
            return this.View("All",this.GetSalesWithCarsModel(null,true));
        }

        [Route("discounted/{percent}")]
        public IActionResult Discounted(double percent)
        {
            return this.ViewOrNotFound(this.GetSalesWithCarsModel(null, true, percent), "All");
        }

        private IEnumerable<SaleListModel> GetSalesWithCarsModel(int? id, bool discounted)
        {
            return this.saleService.All(id,discounted, null);
        }

        private IEnumerable<SaleListModel> GetSalesWithCarsModel(int? id, bool discounted, double percentage)
        {
            return this.saleService.All(id, discounted, percentage);
        }
    }
}

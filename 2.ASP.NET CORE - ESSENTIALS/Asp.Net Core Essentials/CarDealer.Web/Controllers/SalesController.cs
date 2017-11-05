using CarDealer.Services.Contracts;
using CarDealer.Services.Models.Sales;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Controllers
{
    
    [Route("Sales")]
    public class SalesController : Controller
    {
        private readonly ISaleService saleService;

        public SalesController(ISaleService saleService)
        {
            this.saleService = saleService;
        }

        [Route("")]
        public IActionResult All()
        {
            return View(this.GetSalesWithCarsModel(null,false));
        }

        [Route("{id}")]
        public IActionResult All(int id)
        {
            return View(this.GetSalesWithCarsModel(id,false));
        }

        [Route("discounted")]
        public IActionResult Discounted()
        {
            return this.View("All",this.GetSalesWithCarsModel(null,true));
        }

        [Route("discounted/{percent}")]
        public IActionResult Discounted(double percent)
        {
            return this.View("All", this.GetSalesWithCarsModel(null, true, percent));
        }

        private IEnumerable<SalesWithCars> GetSalesWithCarsModel(int? id, bool discounted)
        {
            return this.saleService.All(id,discounted, null);
        }

        private IEnumerable<SalesWithCars> GetSalesWithCarsModel(int? id, bool discounted, double percentage)
        {
            return this.saleService.All(id, discounted, percentage);
        }
    }
}

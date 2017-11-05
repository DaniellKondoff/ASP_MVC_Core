using CarDealer.Services.Contracts;
using CarDealer.Web.Models.CarModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Controllers
{
    [Route("cars")]
    public class CarsController : Controller
    {
        private readonly ICarService carService;

        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

        [Route("{make}",Order = 2)]
        public IActionResult ByMake(string make)
        {
            var cars = this.carService.ByMake(make);

            return View(new CarsByMakeModel
            {
                Cars = cars,
                Make = make
            });
        }

        [Route("parts",Order = 1)]
        public IActionResult Parts()
        {
            return this.View(this.carService.WithParts());
        }
    }
}

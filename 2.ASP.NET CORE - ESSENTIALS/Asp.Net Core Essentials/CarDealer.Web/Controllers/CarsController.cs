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
        private const int PageSize = 25;

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
        public IActionResult Parts(int page =1)
        {

            return this.View( new CarsListingModel
            {
                AllCars = this.carService.WithParts(page, PageSize),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.carService.Total() / (double)PageSize)
            }
            );
        }

        [Route(nameof(Create))]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(Create))]
        public IActionResult Create(CarFormModel carModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            this.carService.Create(carModel.Make, carModel.Model, carModel.TravelledDistance);

            return RedirectToAction(nameof(Parts));
        }
    }
}

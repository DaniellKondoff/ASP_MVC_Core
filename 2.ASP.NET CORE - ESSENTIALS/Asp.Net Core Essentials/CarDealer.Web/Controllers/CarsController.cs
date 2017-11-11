using CarDealer.Services.Contracts;
using CarDealer.Web.Models.CarModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IPartService partService;
        private const int PageSize = 25;

        public CarsController(ICarService carService, IPartService partService)
        {
            this.carService = carService;
            this.partService = partService;
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
            return View(new CarFormModel
            {
                AllParts = this.GetPartsSelectItems()
            });
        }

        [HttpPost]
        [Route(nameof(Create))]
        public IActionResult Create(CarFormModel carModel)
        {
            if (!ModelState.IsValid)
            {
                carModel.AllParts = this.GetPartsSelectItems();
                return View();
            }

            this.carService.Create(carModel.Make, carModel.Model, carModel.TravelledDistance, carModel.SelectedParts) ;

            return RedirectToAction(nameof(Parts));
        }

        private IEnumerable<SelectListItem> GetPartsSelectItems()
        {
            return this.partService.All().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            });
        }
    }
}

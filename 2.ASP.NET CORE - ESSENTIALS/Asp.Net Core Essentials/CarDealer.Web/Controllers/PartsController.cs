using CarDealer.Services.Contracts;
using CarDealer.Web.Models.PartsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Controllers
{
    public class PartsController : Controller
    {
        private const int PageSize = 25;
        private readonly IPartService partService;
        private readonly ISupplierService supplierService;

        public PartsController(IPartService partService, ISupplierService supplierService)
        {
            this.partService = partService;
            this.supplierService = supplierService;
        }

        public IActionResult All(int page = 1)
        {
            return this.View(new PartPageListingModel
            {
                Parts = this.partService.All(page,PageSize),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.partService.Total() / (double)PageSize)
            });
        }

        public IActionResult Create() => View(new PartFormModel
        {
            Suppliers = this.GetSupplierListItems()
        });

        [HttpPost]
        public IActionResult Create(PartFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Suppliers = this.GetSupplierListItems();
                return View(model);
            }

            this.partService.Create(model.Name, model.Price, model.Quantity, model.SupplierId);

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<SelectListItem> GetSupplierListItems()
        {
            return this.supplierService.All().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });
        }
    }
}

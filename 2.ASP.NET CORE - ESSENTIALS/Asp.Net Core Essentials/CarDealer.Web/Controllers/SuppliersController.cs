using CarDealer.Data.Models.Enums;
using CarDealer.Services.Contracts;
using CarDealer.Web.Models.SuppliersModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Controllers
{
    [Authorize]
    public class SuppliersController : Controller
    {
        private readonly ISupplierService supplierService;
        private readonly IPartService partService;
        private readonly ILogService logService;
        private const string SupplierView = "Suppliers";
        private const string SupplierTable = "Suppliers";

        public SuppliersController(ISupplierService supplierService, IPartService partService, ILogService logService)
        {
            this.supplierService = supplierService;
            this.partService = partService;
            this.logService = logService;
        }

        public IActionResult All()
        {
            var allSuppliers = this.supplierService
                .AllBoth()
                .Select(s => new AllSuppliersViewModel
                {
                    Id = s.Id,
                    Importer = s.IsImporter ? "Yes" : "No",
                    Name = s.Name
                })
                .ToList();

            return this.View(allSuppliers);
        }

        [Authorize]
        public IActionResult Create()
        {
            var parts = this.GetAllParts();

            return View(new SupplierFormViewModel
            {
                Parts = parts
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(SupplierFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Parts = this.GetAllParts();
                return View();
            }

            this.supplierService.Create(model.Name, model.IsImporter, model.SelectedParts);

            this.logService.Create(this.User.Identity.Name, Operation.Add, SupplierTable, DateTime.UtcNow);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var supplier = this.supplierService.GetById(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(new SupplierFormViewModel
            {
                Name = supplier.Name,
                IsImporter = supplier.IsImporter,
                Parts = this.GetAllParts(supplier.AllParts.Select(p=>p.Id)),
                IsForEdit = true
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, EditSupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Parts = this.GetAllParts(model.SelectedParts);
                return View();
            }

            this.supplierService.Edit(id, model.Name, model.IsImporter, model.SelectedParts);

            this.logService.Create(this.User.Identity.Name, Operation.Edit, SupplierTable, DateTime.UtcNow);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Delete(int id) => View(id);

        [Authorize]
        public IActionResult Destroy(int id)
        {
            this.supplierService.Delete(id);

            this.logService.Create(this.User.Identity.Name, Operation.Delete, SupplierTable, DateTime.UtcNow);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Local()
        {
            return this.View(SupplierView, this.GetSuppliersModel(false));
        }

        public IActionResult Importers()
        {
            return this.View(SupplierView, this.GetSuppliersModel(true));
        }

        private SuppliersModel GetSuppliersModel(bool importers)
        {
            var type = importers ? "Importer" : "Local";
            var suppliers = this.supplierService.AllListing(importers);

            return new SuppliersModel
            {
                Type = type,
                Suppliers = suppliers
            };
        }

        private IEnumerable<SelectListItem> GetAllParts()
        {
          return  this.partService.All()
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                });
        }

        private IEnumerable<SelectListItem> GetAllParts(IEnumerable<int> partsId)
        {
            return this.partService.All()
                  .Select(p => new SelectListItem
                  {
                      Text = p.Name,
                      Value = p.Id.ToString(),
                      Selected =  partsId.Contains(p.Id)
                  });
        }
    }
}

using CarDealer.Services.Contracts;
using CarDealer.Web.Models.SuppliersModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISupplierService supplierService;
        private const string SupplierView = "Suppliers";

        public SuppliersController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
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
            var suppliers = this.supplierService.All(importers);

            return new SuppliersModel
            {
                Type = type,
                Suppliers = suppliers
            };
        }
    }
}

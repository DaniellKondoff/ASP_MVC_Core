using CarDealer.Services.Models.Suppliers;
using System.Collections.Generic;

namespace CarDealer.Web.Models.SuppliersModels
{
    public class SuppliersModel
    {
        public string Type { get; set; }

        public IEnumerable<SupplierListingModel> Suppliers { get; set; }
    }
}

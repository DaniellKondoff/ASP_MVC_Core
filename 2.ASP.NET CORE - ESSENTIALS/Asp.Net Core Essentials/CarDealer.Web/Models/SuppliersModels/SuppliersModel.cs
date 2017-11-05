using CarDealer.Services.Models;
using System.Collections.Generic;

namespace CarDealer.Web.Models.SuppliersModels
{
    public class SuppliersModel
    {
        public string Type { get; set; }

        public IEnumerable<SupplierModel> Suppliers { get; set; }
    }
}

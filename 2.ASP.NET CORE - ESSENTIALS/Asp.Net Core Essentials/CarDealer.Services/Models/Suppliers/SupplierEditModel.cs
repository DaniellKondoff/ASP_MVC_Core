using CarDealer.Services.Models.Parts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Models.Suppliers
{
    public class SupplierEditModel
    {
        public string Name { get; set; }

        public bool IsImporter { get; set; }

        public IEnumerable<PartsShortModel> AllParts { get; set; }
    }
}

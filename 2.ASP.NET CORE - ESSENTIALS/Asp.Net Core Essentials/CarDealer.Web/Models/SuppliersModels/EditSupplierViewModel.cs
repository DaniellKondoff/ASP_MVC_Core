using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.Web.Models.SuppliersModels
{
    public class EditSupplierViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "Is Importer")]
        public bool IsImporter { get; set; }

        public IEnumerable<int> SelectedParts { get; set; }

        [Display(Name = "Parts")]
        public IEnumerable<SelectListItem> Parts { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Models.SalesViewModels
{
    public class CreateSaleViewModel
    {
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        [Display(Name = "Car")]
        public int CarId { get; set; }

        public IEnumerable<SelectListItem> Cars { get; set; }

        [Display(Name = "Discount")]
        public int DiscountId { get; set; }

        public IEnumerable<SelectListItem> Discounts { get; set; }

    }
}

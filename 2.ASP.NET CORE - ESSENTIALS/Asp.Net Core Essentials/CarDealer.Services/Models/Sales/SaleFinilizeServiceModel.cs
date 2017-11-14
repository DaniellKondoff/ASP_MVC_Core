﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarDealer.Services.Models.Sales
{
    public class SaleFinilizeServiceModel
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CustomerName { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        [MaxLength(150)]
        public string CarName { get; set; }

        [Required]
        public int DiscountId { get; set; }

        [Range(0, 100)]
        public double Discount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CarPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal FinalCarPrice { get; set; }
    }
}

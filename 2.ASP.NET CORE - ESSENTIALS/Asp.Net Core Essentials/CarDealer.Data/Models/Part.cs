using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarDealer.Data.Models
{
    public class Part
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(0,double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0,double.MaxValue)]
        public double Quantity { get; set; }

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        public List<PartCars> Cars { get; set; } = new List<PartCars>();
    }
}

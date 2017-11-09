﻿using System.ComponentModel.DataAnnotations;

namespace CarDealer.Web.Models.CarModels
{
    public class CarFormModel
    {
        [Required]
        [MaxLength(50)]
        public string Make { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        [Display(Name ="Travelled Distance")]
        [Range(0, long.MaxValue, ErrorMessage = "{2} must be Positive Number")]
        public long TravelledDistance { get; set; }
    }
}

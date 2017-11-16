using CameraBazaar.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CameraBazaar.Services.Models.Cameras
{
    public class AllListingServiceModel
    {
        public int Id { get; set; }

        public CameraMakeType Make { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        public decimal Price { get; set; }

        public bool IsInStock { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public string ImageUrl { get; set; }

    }
}

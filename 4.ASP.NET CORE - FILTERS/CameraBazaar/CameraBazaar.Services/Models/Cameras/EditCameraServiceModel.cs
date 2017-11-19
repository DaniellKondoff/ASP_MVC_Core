using CameraBazaar.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CameraBazaar.Services.Models.Cameras
{
    public class EditCameraServiceModel
    {
        public CameraMakeType Make { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        public decimal Price { get; set; }

        [Range(0, 100)]
        public int Quantity { get; set; }

        [Range(1, 30)]
        public int MinShutterSpeed { get; set; }

        [Range(2000, 8000)]
        public int MaxShutterSpeed { get; set; }


        public MinIsoType MinISO { get; set; }

        [Range(200, 409600)]
        public int MaxISO { get; set; }

        public bool IsFullFrame { get; set; }

        [Required]
        [MaxLength(15)]
        public string VideoResolution { get; set; }

        public LightMetering LightMetering { get; set; }

        [Required]
        [MaxLength(6000)]
        public string Description { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public string ImageUrl { get; set; }
    }
}

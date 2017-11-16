using CameraBazaar.Services.Contracts;
using System;
using CameraBazaar.Data.Models.Enums;
using CameraBazaar.Data;
using CameraBazaar.Data.Models;
using System.Collections.Generic;
using System.Linq;
using CameraBazaar.Services.Models.Cameras;

namespace CameraBazaar.Services.Implementations
{
    public class CameraService : ICameraService
    {
        private readonly CameraBazaarDbContext db;

        public CameraService(CameraBazaarDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<AllListingServiceModel> AllListing()
        {
            return this.db
                .Cameras
                .Select(c => new AllListingServiceModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    Make = c.Make,
                    Price = c.Price,
                    ImageUrl = c.ImageUrl,
                    IsInStock = c.Quantity > 0 ? true : false
                })
                .ToList();
        }

        public void Create(CameraMakeType make, 
            string model, 
            decimal price, 
            int quantity, 
            int minShutterSpeed, 
            int maxShutterSpeed, 
            MinIsoType minIso, 
            int maxIso, 
            bool isFullFrame, 
            string videoResolution,
            IEnumerable<LightMetering> lightMeterings, 
            string description, 
            string imageUrl, 
            string userId)
        {
            var camera = new Camera
            {
                Make = make,
                Model = model,
                Price = price,
                Quantity = quantity,
                MinShutterSpeed = minShutterSpeed,
                MaxShutterSpeed = maxShutterSpeed,
                MinISO = minIso,
                MaxISO = maxIso,
                IsFullFrame = isFullFrame,
                VideoResolution = videoResolution,
                LightMetering = (LightMetering)lightMeterings.Cast<int>().Sum(),
                Description = description,
                ImageUrl = imageUrl,
                UserId = userId
                
            };

            this.db.Add(camera);
            this.db.SaveChanges();
        }

        public DetailsCameraServiceModel Details(int id)
        {
            return db.Cameras
                .Where(c => c.Id == id)
                .Select(c => new DetailsCameraServiceModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    Price = c.Price,
                    Quantity = c.Quantity,
                    MinShutterSpeed = c.MinShutterSpeed,
                    MaxShutterSpeed = c.MaxShutterSpeed,
                    MinISO = c.MinISO,
                    MaxISO = c.MaxISO,
                    IsFullFrame = c.IsFullFrame,
                    VideoResolution = c.VideoResolution,
                    LightMetering = c.LightMetering,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    UserId = c.UserId,
                    Username = c.User.UserName,
                    IsInStock = (c.Quantity > 0) ? true : false
                })
                .FirstOrDefault();
        }
    }
}

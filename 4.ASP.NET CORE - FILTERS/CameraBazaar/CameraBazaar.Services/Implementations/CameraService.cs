using CameraBazaar.Data;
using CameraBazaar.Data.Models;
using CameraBazaar.Data.Models.Enums;
using CameraBazaar.Services.Contracts;
using CameraBazaar.Services.Models.Cameras;
using System.Collections.Generic;
using System.Linq;

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

        public void Delete(int id)
        {
            var camera = this.db.Cameras.Find(id);

            if (camera == null)
            {
                return;
            }

            this.db.Cameras.Remove(camera);
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

        public void Edit(int id, CameraMakeType make, string model, decimal price, int quantity, int minShutterSpeed, int maxShutterSpeed, MinIsoType minISO, int maxISO, bool isFullFrame, string videoResolution, LightMetering lightMetering, string description, string imageUrl)
        {
            var camera = this.db.Cameras.Find(id);
            if (camera == null)
            {
                return;
            }

            camera.Make = make;
            camera.Model = model;
            camera.Price = price;
            camera.Quantity = quantity;
            camera.MinShutterSpeed = minShutterSpeed;
            camera.MaxShutterSpeed = maxShutterSpeed;
            camera.MinISO = minISO;
            camera.MaxISO = maxISO;
            camera.IsFullFrame = isFullFrame;
            camera.VideoResolution = videoResolution;
            camera.LightMetering = lightMetering;
            camera.Description = description;
            camera.ImageUrl = imageUrl;

            this.db.SaveChanges();
        }

        public EditCameraServiceModel GetById(int id)
        {
            return this.db.Cameras
                .Where(c => c.Id == id)
                .Select(c => new EditCameraServiceModel
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
                })
                .FirstOrDefault();
        }
    }
}

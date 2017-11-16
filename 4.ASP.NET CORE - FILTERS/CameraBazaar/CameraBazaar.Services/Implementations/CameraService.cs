﻿using CameraBazaar.Services.Contracts;
using System;
using CameraBazaar.Data.Models.Enums;
using CameraBazaar.Data;
using CameraBazaar.Data.Models;
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
    }
}
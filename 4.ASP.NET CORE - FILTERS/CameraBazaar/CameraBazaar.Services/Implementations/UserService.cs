using CameraBazaar.Data;
using CameraBazaar.Services.Contracts;
using CameraBazaar.Services.Models.Cameras;
using CameraBazaar.Services.Models.Users;
using System;
using System.Linq;

namespace CameraBazaar.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly CameraBazaarDbContext db;

        public UserService(CameraBazaarDbContext db)
        {
            this.db = db;
        }


        public UserFormServiceModel GetById(string id)
        {
                var user =  this.db.Users
                .Where(u => u.Id == id)
                .Select(u => new UserFormServiceModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Password = u.PasswordHash,
                    Phone = u.PhoneNumber
                })
                .FirstOrDefault();

            return user;
        }

        public UserDetailsListingModel GetUser(string id)
        {
            return this.db.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDetailsListingModel
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email,
                    Phone = u.PhoneNumber,
                    CamerasCollection = u.Cameras.Select(c => new CameraUserColectionServiceModel
                    {
                        Id = c.Id,
                        Make = c.Make.ToString(),
                        Model = c.Model,
                        Price = c.Price,
                        IsInStock = c.Quantity > 1 ? true : false,
                        ImageUrl = c.ImageUrl
                    }),
                    Cameras = $"{u.Cameras.Count(c=> c.Quantity > 0)} in stock / {u.Cameras.Count(c => c.Quantity <= 0)} out of stock",
                    LastLogin = u.LastLogin
                })
                .FirstOrDefault();
        }

        public void WriteLastLogin(string id)
        {
            var user = this.db.Users.Find(id);

            if (user == null)
            {
                return;
            }

            user.LastLogin = DateTime.Now;
            this.db.SaveChanges();
        }
    }
}

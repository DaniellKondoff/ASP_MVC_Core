using CameraBazaar.Services.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace CameraBazaar.Services.Contracts
{
    public interface IUserService
    {
        UserDetailsListingModel GetUser(string id);

        UserFormServiceModel GetById(string id);

        void WriteLastLogin(string id);
    }
}

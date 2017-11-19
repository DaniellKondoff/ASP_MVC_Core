using CameraBazaar.Services.Models.Cameras;
using System.Collections.Generic;

namespace CameraBazaar.Services.Models.Users
{
    public class UserDetailsListingModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Cameras { get; set; }

        public IEnumerable<CameraUserColectionServiceModel> CamerasCollection { get; set; }
    }
}

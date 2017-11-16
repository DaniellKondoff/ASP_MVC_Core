using CameraBazaar.Data.Models.Enums;
using CameraBazaar.Services.Models.Cameras;
using System.Collections.Generic;

namespace CameraBazaar.Services.Contracts
{
    public interface ICameraService
    {
        void Create(
            CameraMakeType make,
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
            string userId);

        IEnumerable<AllListingServiceModel> AllListing();

        DetailsCameraServiceModel Details(int id);
    }
}

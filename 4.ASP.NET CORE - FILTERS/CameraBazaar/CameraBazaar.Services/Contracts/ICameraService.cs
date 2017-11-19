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

        EditCameraServiceModel GetById(int id);

        void Edit(int id, CameraMakeType make, string model, decimal price, int quantity, int minShutterSpeed, int maxShutterSpeed, MinIsoType minISO, int maxISO, bool isFullFrame, string videoResolution, LightMetering lightMetering, string description, string imageUrl);

        void Delete(int id);
    }
}

using CarDealer.Services.Models.Cars;
using System.Collections.Generic;

namespace CarDealer.Web.Models.CarModels
{
    public class CarsByMakeModel
    {
        public string Make { get; set; }

        public IEnumerable<CarByMakeModel> Cars { get; set; }
    }
}

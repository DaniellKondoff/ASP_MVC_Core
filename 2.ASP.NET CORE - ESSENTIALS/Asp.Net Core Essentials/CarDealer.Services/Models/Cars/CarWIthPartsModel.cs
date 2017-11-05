using CarDealer.Services.Models.Parts;
using System.Collections.Generic;

namespace CarDealer.Services.Models.Cars
{
    public class CarWIthPartsModel : CarByMakeModel
    {
        public IEnumerable<PartModel> Parts { get; set; }
    }
}

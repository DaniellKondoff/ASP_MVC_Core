using CarDealer.Services.Models.Cars;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Contracts
{
    public interface ICarService
    {
        IEnumerable<CarByMakeModel> ByMake(string make);

        IEnumerable<CarWIthPartsModel> WithParts();
    }
}

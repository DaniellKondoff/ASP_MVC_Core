using CarDealer.Services.Models.Cars;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Contracts
{
    public interface ICarService
    {
        IEnumerable<CarByMakeModel> ByMake(string make);

        IEnumerable<CarsBasicModel> AllBasic();

        IEnumerable<CarWIthPartsModel> WithParts(int page = 1, int pageSize = 10);

        void Create(string make, string model, long travelledDistance, IEnumerable<int> parts);

        int Total();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CarDealer.Data.Models.Enums;
using CarDealer.Services.Models.Logs;

namespace CarDealer.Services.Contracts
{
    public interface ILogService
    {
        void Create(string userName, Operation operationType, string table, DateTime modifiedDate);

        IEnumerable<LogsListingServiceModel> AllListing();
        void Clear();
    }
}

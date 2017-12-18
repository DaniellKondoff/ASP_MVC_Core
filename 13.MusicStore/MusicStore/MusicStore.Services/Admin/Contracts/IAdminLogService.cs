using MusicStore.Data.Enums;
using MusicStore.Services.Admin.Models.Logs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStore.Services.Admin.Contracts
{
    public interface IAdminLogService
    {
        Task Create(string userName, Operation operationType, string table, DateTime modifiedDate);

        Task<IEnumerable<LogsListingServiceModel>> AllListingAsync(int page);

        Task ClearAsync();

        Task<int> TotalAsync();
    }
}

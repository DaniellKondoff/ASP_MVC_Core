using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Data.Enums;
using MusicStore.Data.Models;
using MusicStore.Services.Admin.Contracts;
using System;
using System.Threading.Tasks;
using MusicStore.Services.Admin.Models.Logs;
using System.Collections.Generic;
using System.Linq;
using static MusicStore.Services.ServiceConstants;
using AutoMapper.QueryableExtensions;

namespace MusicStore.Services.Admin.Implementations
{
    public class AdminLogService : IAdminLogService
    {
        private readonly MusicStoreDbContext db;
        public AdminLogService(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<LogsListingServiceModel>> AllListingAsync(int page)
        {
            return await this.db
                .Logs
                .OrderByDescending(a => a.Id)
                .Skip((page - 1) * AdminLogsListingPageSize)
                .Take(AdminLogsListingPageSize)
                .ProjectTo<LogsListingServiceModel>()
                .ToListAsync();
        }

        public async Task ClearAsync()
        {
            var logs = await this.db.Logs.ToListAsync();
            this.db.Logs.RemoveRange(logs);

            await this.db.SaveChangesAsync();
        }

        public async Task Create(string userName, Operation operationType, string table, DateTime modifiedDate)
        {
            var log = new Log
            {
                UserName = userName,
                LogOperation = operationType,
                ModifiedTable = table,
                ModifiedOn = modifiedDate
            };

            this.db.Logs.Add(log);
            await this.db.SaveChangesAsync();
        }

        public async Task<int> TotalAsync()
        {
            return await this.db.Logs.CountAsync();
        }
    }
}

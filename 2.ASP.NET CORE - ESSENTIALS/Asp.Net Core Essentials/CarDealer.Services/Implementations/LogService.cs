using CarDealer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using CarDealer.Data.Models.Enums;
using CarDealer.Data.Models;
using CarDealer.Data;
using CarDealer.Services.Models.Logs;
using System.Linq;

namespace CarDealer.Services.Implementations
{
    public class LogService : ILogService
    {
        private readonly CarDealerDbContext db;

        public LogService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<LogsListingServiceModel> AllListing()
        {
            return this.db.Logs
                .OrderByDescending(l => l.Id)
                .Select(l => new LogsListingServiceModel
                {
                    UserName = l.UserName,
                    OperationType = Enum.GetName(typeof(Operation), l.LogOperation),
                    ModifiedTable = l.ModifiedTable,
                    ModifiedDate = l.ModifiedOn
                })
                .ToList();
        }

        public void Clear()
        {
            var logs = this.db.Logs.ToList();
            this.db.Logs.RemoveRange(logs);

            this.db.SaveChanges();
        }

        public void Create(string userName, Operation operationType, string table, DateTime modifiedDate)
        {
            var log = new Log
            {
                UserName = userName,
                LogOperation = operationType,
                ModifiedTable = table,
                ModifiedOn = modifiedDate
            };

            this.db.Logs.Add(log);
            this.db.SaveChanges();
        }

        public int Total() => this.db.Logs.Count();
        
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Services.Models.Logs
{
    public class LogsListingServiceModel
    {
        public string UserName { get; set; }

        public string OperationType { get; set; }

        public string ModifiedTable { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}

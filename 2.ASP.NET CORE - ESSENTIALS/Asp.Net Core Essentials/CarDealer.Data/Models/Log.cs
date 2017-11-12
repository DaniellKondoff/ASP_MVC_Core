using CarDealer.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Data.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public Operation LogOperation { get; set; }

        public string ModifiedTable { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}

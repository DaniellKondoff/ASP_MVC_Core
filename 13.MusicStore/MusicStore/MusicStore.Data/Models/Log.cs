using MusicStore.Data.Enums;
using System;

namespace MusicStore.Data.Models
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

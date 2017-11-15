﻿using CarDealer.Services.Models.Logs;
using System.Collections.Generic;

namespace CarDealer.Web.Models.LogsViewModels
{
    public class LogsListingViewModel
    {
        public IEnumerable<LogsListingServiceModel> AllLogs { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;

        public string Search { get; set; }
    }
}
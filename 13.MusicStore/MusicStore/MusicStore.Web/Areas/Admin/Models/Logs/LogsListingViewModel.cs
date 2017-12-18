using MusicStore.Services;
using MusicStore.Services.Admin.Models.Logs;
using System;
using System.Collections.Generic;

namespace MusicStore.Web.Areas.Admin.Models.Logs
{
    public class LogsListingViewModel
    {
        public IEnumerable<LogsListingServiceModel> AllLogs { get; set; }

        public int TotalLogs { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalLogs/ ServiceConstants.AdminLogsListingPageSize);

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CameraBazaar.Web.Infrastructure.Filters
{
    public class MeassureTimeAttribute : ActionFilterAttribute
    {
        private Stopwatch stopwatch;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.stopwatch = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            this.stopwatch.Stop();

            using (var writer = new StreamWriter("action-times.txt",true))
            {
                var dateTime = DateTime.UtcNow;
                var controller = context.Controller.GetType().Name;
                var action = context.RouteData.Values["action"];

                var logData = $"{dateTime} - {controller}.{action} - {this.stopwatch.Elapsed}";

                writer.WriteLine(logData);
            }
        }
    }
}

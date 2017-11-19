using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace CameraBazaar.Web.Infrastructure.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            using (var writer = new StreamWriter("logs.txt",true))
            {
                var dateTime = DateTime.UtcNow;
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress;
                var user = context.HttpContext.User?.Identity?.Name ?? "Anonymos";
                var controller = context.Controller.GetType().Name;
                var action = context.RouteData.Values["action"];

                var logData = $"{dateTime} - {ipAddress} - {user} - {controller}.{action}";

                if (context.Exception != null)
                {
                    var exceptionType = context.Exception.GetType().Name;
                    var exceptyionMessage = context.Exception.Message;
                    logData = $"[!] {logData} - {exceptionType} - {exceptyionMessage}";
                }

                writer.WriteLine(logData);
            }
            
        }
    }
}

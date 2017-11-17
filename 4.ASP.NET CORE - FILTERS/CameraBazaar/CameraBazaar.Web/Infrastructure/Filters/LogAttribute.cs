using CameraBazaar.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CameraBazaar.Web.Infrastructure.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        private string controller;
        private string action;

        public LogAttribute(string controller, string action)
        {
            this.controller = controller;
            this.action = action;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var dateTime = DateTime.UtcNow.ToLongDateString();
            var ipAddress = context.HttpContext.Connection.LocalIpAddress;
            var user = context.HttpContext.User.Identity.IsAuthenticated ? context.HttpContext.User.Identity.Name : "Anonymos";

            var logService = context.HttpContext.RequestServices.GetService<ILogService>();
            var logData = $"{dateTime} - {ipAddress} - {user} - {controller}.{action}";

            if (context.ExceptionHandled)
            {

            }
            logService.Create(logData);
        }

        
    }
}

using CarDealer.Data;
using CarDealer.Data.Models.Enums;
using CarDealer.Services.Contracts;
using CarDealer.Services.Implementations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CarDealer.Web.Infrastructure.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        private Operation operationType;
        private string tableName;
        public LogAttribute(Operation operationtType, string tableName)
        {
            if (tableName == null)
            {
                throw new ArgumentNullException();
            }

            this.operationType = operationtType;
            this.tableName = tableName;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var userName = context.HttpContext.User.Identity.Name;
            var logService = context.HttpContext.RequestServices.GetService<ILogService>();

            logService.Create(userName, operationType, tableName, DateTime.UtcNow);
        }
    }
}

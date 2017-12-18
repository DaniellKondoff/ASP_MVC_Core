using Microsoft.AspNetCore.Mvc.Filters;
using MusicStore.Data.Enums;
using System;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.Services.Admin.Contracts;

namespace MusicStore.Web.Infrastructure.Filters
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
            var logService = context.HttpContext.RequestServices.GetService<IAdminLogService>();

            logService.Create(userName, operationType, tableName, DateTime.UtcNow);
        }
    }
}

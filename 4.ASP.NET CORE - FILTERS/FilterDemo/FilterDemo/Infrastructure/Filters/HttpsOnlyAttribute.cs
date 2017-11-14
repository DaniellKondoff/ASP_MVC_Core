using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FilterDemo.Infrastructure.Filters
{
    public class HttpsOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.IsHttps)
            {
                var request = context.HttpContext.Request;
                var url = $"https://{request.Host}/{request.Path}?{request.Query}";

                context.Result = new RedirectResult(url);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FilterDemo.Infrastructure.Filters
{
    public class RedirectExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly Type exceptionType;

        public RedirectExceptionAttribute(Type exceptionType)
        {
            this.exceptionType = exceptionType;
        }

        public override void OnException(ExceptionContext context)
        {
            if (this.exceptionType == null || exceptionType == context.Exception.GetType())
            {
                context.Result = new RedirectResult("/");
            }
        }
    }
}

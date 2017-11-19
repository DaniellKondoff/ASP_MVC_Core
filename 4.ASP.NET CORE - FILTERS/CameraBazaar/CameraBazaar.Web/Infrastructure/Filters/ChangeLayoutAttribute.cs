using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraBazaar.Web.Infrastructure.Filters
{
    public class ChangeLayoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            var result = context.Result as ViewResult;

            if (result != null)
            {
                result.ViewName = "_UserLayout";
            }
        }

    }
}

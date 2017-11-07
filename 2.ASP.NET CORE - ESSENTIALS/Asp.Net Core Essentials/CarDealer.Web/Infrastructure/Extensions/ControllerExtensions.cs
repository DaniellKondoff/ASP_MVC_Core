using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Web.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ViewOrNotFound(this Controller controller, object model)
        {
            if (model == null)
            {
                return controller.NotFound();
            }

            return controller.View(model);
        }

        public static IActionResult ViewOrNotFound(this Controller controller, object model, string viewName)
        {
            if (model == null)
            {
                return controller.NotFound();
            }

            return controller.View(viewName, model);
        }
    }
}

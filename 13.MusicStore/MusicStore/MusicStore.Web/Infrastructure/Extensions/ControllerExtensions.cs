using Microsoft.AspNetCore.Mvc;

namespace MusicStore.Web.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ViewOrNotFounr(this Controller controller, object model)
        {
            if (model == null)
            {
                return controller.NotFound();
            }

            return controller.View(model);
        }
    }
}

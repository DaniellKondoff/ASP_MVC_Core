using Microsoft.AspNetCore.Mvc;

namespace AreasDemo.Areas.Products.Controllers
{

    public class HomeController : ProductsBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

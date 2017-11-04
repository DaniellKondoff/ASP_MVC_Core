using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWebApp.Controllers
{
    public class CatsController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Create), new { name = "Vankata" });
        }

    }
}

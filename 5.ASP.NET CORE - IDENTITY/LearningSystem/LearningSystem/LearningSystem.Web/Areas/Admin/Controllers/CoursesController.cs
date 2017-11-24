using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    public class CoursesController : BaseAdminController
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}

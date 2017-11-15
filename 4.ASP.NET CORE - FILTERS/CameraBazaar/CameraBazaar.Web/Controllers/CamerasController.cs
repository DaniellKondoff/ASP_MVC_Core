using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraBazaar.Web.Controllers
{
    [Authorize]
    public class CamerasController : Controller
    {
        public IActionResult Add() => this.View();
    }
}

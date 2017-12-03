using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    public class TestController
    {
        //[http]
        public string[] GetText => new[] { "Test", "Text" };

    }
}

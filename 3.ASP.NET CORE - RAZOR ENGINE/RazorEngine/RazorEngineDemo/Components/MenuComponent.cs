using Microsoft.AspNetCore.Mvc;
using RazorEngineDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorEngineDemo.Components
{
    public class MenuComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public MenuComponent(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IViewComponentResult Invoke( int id)
        {
            return this.View();
        }
    }
}

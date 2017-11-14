using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDemo.Infrastructure.Filters
{
    public class AddHeaderAttribute : ResultFilterAttribute
    {
        private string name;
        private string value;

        public AddHeaderAttribute(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var responseHeaders = context.HttpContext.Response.Headers;

            if (!responseHeaders.ContainsKey(this.name))
            {
                responseHeaders.Add(this.name, this.value);
            }
        }
    }
}

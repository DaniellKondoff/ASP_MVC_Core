using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsServer.Infrastructure.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void Redirect(this HttpResponse response,string url)
        {
            response.StatusCode = HttpStatusCode.Found;
            response.Headers.Add(HttpHeader.Location, url);
        }
    }
}

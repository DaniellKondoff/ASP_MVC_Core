using CarDealer.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CarDealer.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionsExensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection service)
        {
            Assembly
                 .GetAssembly(typeof(IService))
                 .GetTypes()
                 .Where(t => t.IsClass && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                 .Select(t => new
                 {
                     Interface = t.GetInterface($"I{t.Name}"),
                     Implementation = t
                 })
                 .ToList()
                 .ForEach(s => service.AddTransient(s.Interface, s.Implementation));

            return service;
        }
    }
}

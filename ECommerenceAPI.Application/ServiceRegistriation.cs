using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application
{
    public static class ServiceRegistriation 
    {
        public static void AddApplicationService(this IServiceCollection collection)
        {
            //collection.AddMediatR(typeof(ServiceRegistriation)); // mediatr kendi buluyor
            collection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        }
    }
}

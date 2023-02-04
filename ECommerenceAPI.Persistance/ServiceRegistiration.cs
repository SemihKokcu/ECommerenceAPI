using Microsoft.EntityFrameworkCore;
using ECommerenceAPI.Persistance.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Persistance.Repositories;

namespace ECommerenceAPI.Persistance
{
    public static class ServiceRegistiration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<ECommerenceAPIDbContext>(options => options.UseNpgsql(Configration.ConnectionString),
            ServiceLifetime.Scoped
                );


            services.AddScoped<ICustomerReadRepository,CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>(); 

        }
    }
}

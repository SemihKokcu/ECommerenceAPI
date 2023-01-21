using Microsoft.EntityFrameworkCore;
using ECommerenceAPI.Persistance.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance
{
    public static class ServiceRegistiration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<ECommerenceAPIDbContext>(options =>
                options.UseNpgsql(Configration.ConnectionString)
            
            );
        }
    }
}

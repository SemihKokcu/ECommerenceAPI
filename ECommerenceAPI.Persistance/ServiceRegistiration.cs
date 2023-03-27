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
using ECommerenceAPI.Domain.Entities.Identity;
using ECommerenceAPI.Application.Abstractions.Services;
using ECommerenceAPI.Persistance.Services;
using ECommerenceAPI.Application.Abstractions.Services.Authentication;

namespace ECommerenceAPI.Persistance
{
    public static class ServiceRegistiration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<ECommerenceAPIDbContext>(options => options.UseNpgsql(Configration.ConnectionString),
            ServiceLifetime.Scoped
                );
            services.AddIdentity<AppUser, AppRole>(
                options => 
                {
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                }
                ).AddEntityFrameworkStores<ECommerenceAPIDbContext>();


            services.AddScoped<ICustomerReadRepository,CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();

            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuthentication,AuthService>();
            services.AddScoped<IInternalAuthentication, AuthService>();

        }
    }
}

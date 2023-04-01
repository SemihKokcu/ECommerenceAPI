using ECommerenceAPI.Application.Abstractions.Hubs;
using ECommerenceAPI.SignalR.HubsServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.SignalR
{
    public static class ServiceRegistiration
    {
        public static void AddSignalRServices(this IServiceCollection collection) 
        {
            collection.AddTransient<IProductHubService, ProductHubService>();
            collection.AddSignalR();


        }
    }
}

using ECommerenceAPI.Application.Abstractions.Storage;
using ECommerenceAPI.Infrastructure.Enums;
using ECommerenceAPI.Infrastructure.Services.Storage;
using ECommerenceAPI.Infrastructure.Services.Storage.Azure;
using ECommerenceAPI.Infrastructure.Services.Storage.Local;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
        }
        //daha sağlıklı
        public static void AddStorage<T> (this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage,T>();
        }

        //kullanım şekli --> tam doğru bir kullanış değildir
        public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
        {
            switch (storageType) 
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage,LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    break;
                default:
                    serviceCollection.AddScoped<IStorage,LocalStorage>();
                    break;
            }
        }
    }
}

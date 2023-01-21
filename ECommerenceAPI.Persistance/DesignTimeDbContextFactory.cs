using ECommerenceAPI.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ECommerenceAPIDbContext>
    {
        public ECommerenceAPIDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ECommerenceAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}

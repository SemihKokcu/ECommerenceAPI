using ECommerenceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance.Contexts
{
    public class ECommerenceAPIDbContext : DbContext
    {
        public ECommerenceAPIDbContext(DbContextOptions options) : base(options)
        {
            //ıoc de doldurulacak
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
}

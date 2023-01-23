using ECommerenceAPI.Domain.Entities;
using ECommerenceAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        //gelen isteklerde araya girme
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //değişiklileri yakalamak için
            //ChangeTracker : entitiy üzerinden yapılan değişiklikleirn ya da yeni ekleen bir şeyin yakalanmasını sağlar Track edilen verileri yakalar
            var datas = ChangeTracker
                .Entries<BaseEntity>();
            foreach (var data in datas)
            {
                // _ her hangi bir atama yapılmayacak anlamına gelir
              _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreateDate=DateTime.UtcNow,
                    EntityState.Modified =>data.Entity.UpdateDate=DateTime.UtcNow,
                };

            }
            //artık otomatik olarak ekleme yapacak
                return await base.SaveChangesAsync(cancellationToken);
        }

    }
}

using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Domain.Entities.Common;
using ECommerenceAPI.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ECommerenceAPIDbContext _context;

        public ReadRepository(ECommerenceAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();// db set türünden bir nesne dönecek

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        } // table tabloyu set edicek
        
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }
       
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }

        //marker pattern: Bizim base entitiy miz var olan bütün entitiylerde olması gereken id yi tutar 
        // biz de sınıfımıza sen bir :class yerine sen bir :BaseEntity olduğunu söylersek hem class olduğunu alabiliriz
        // hem de buradan bir id sorgulama işlemi yapabiliriz
        // IRead ve IWrite lara da böyle olduğunu söyle

        //public async Task<T> GetByIdAsync(string id)
        //=> await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));

        // ya da findbyasync ile de yapılabilir

        //findasync yapabilrii fakat IQueryable da bu yoktur
        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if(!tracking)
                query=query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data=>data.Id==Guid.Parse(id)); 
        }
    }
}

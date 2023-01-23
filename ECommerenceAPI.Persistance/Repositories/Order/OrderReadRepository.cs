using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Domain.Entities;
using ECommerenceAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance.Repositories
{
    public class OrderReadRepository : ReadRepository<Order>, IOrderReadRepository
    {
        public OrderReadRepository(ECommerenceAPIDbContext context) : base(context)
        {
        }
    }
}

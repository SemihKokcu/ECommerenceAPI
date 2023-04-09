﻿using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Domain.Entities;
using ECommerenceAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance.Repositories
{
    public class BasketItemReadRepository : ReadRepository<BasketItem>, IBasketItemReadRepository
    {
        public BasketItemReadRepository(ECommerenceAPIDbContext context) : base(context)
        {
        }
    }
}

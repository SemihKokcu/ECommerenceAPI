﻿using ECommerenceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Repositories
{
    //select sorgularımız
    public interface IReadRepository<T>: IRepository<T> where T : BaseEntity 
    {
        //sorgu üzerinde çalışmak için IQueryable'dır, veritabanı sorgusuna eklenir
        //Task : asenkron veri tipi döndürür

        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T,bool>> method, bool tracking = true);
        Task<T> GetSingleAsync(Expression<Func<T,bool>> method, bool tracking = true);
        Task<T> GetByIdAsync(string id, bool tracking = true);

    
    
    }
}
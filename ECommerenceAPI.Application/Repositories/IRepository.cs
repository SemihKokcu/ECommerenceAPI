using ECommerenceAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Repositories
{
    // evrensel metotları burada tutarız base olacak
    public interface IRepository<T> where T : BaseEntity  // DbSet : class entitiy ister o da BaseEntity
    {
        DbSet<T> Table { get; } // table alırız fakat set yapmayız
    }
}

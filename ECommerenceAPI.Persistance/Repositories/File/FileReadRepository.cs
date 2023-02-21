using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance.Repositories
{
    public class FileReadRepository : ReadRepository<ECommerenceAPI.Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(ECommerenceAPIDbContext context) : base(context)
        {
        }
    }
}

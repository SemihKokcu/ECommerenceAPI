using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance.Repositories
{
    public class FileWriteRepository : WriteRepository<ECommerenceAPI.Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(ECommerenceAPIDbContext conext) : base(conext)
        {
        }
    }
}

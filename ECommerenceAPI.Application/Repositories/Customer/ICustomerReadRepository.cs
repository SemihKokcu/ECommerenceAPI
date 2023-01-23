using ECommerenceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace'lerden Customer, Order gibi klasör adlarını sildik namspace lerde sorun olmasın diye
//klaösrlemede yönetirken ismini verdik
namespace ECommerenceAPI.Application.Repositories
{
    public interface ICustomerReadRepository : IReadRepository<Customer>
    {
    }
}

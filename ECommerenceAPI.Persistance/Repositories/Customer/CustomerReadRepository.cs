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
    /* 
        interface içerisinde de generic yapıya customer vererek bu işlemleri sağlayabilirdik fakat zaten bunu generic olarak içinde barındırdğı
        metotları veriyoruz. Bunu da read repository üzerinden kullanıma erişim verdik. Ioc den de bu nesnenin yaratılabilmesi ve metolarını kullanabilmek için
        ona ICustomerReadRepository olduğunu söylüyoruz. Böylece bu sınıf ve interface ler birnirini biliyor ve tutuyor
     */
    public class CustomerReadRepository : ReadRepository<Customer>, ICustomerReadRepository
    {
        public CustomerReadRepository(ECommerenceAPIDbContext context) : base(context)
        {
        }
    }
}

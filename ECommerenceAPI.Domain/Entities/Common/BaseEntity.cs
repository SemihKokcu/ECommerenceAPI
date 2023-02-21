using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Domain.Entities.Common
{
    public class BaseEntity
    {
        // Guid: veri tabanında uniq identitfy verileri c# da bu şekilde temsil ediyoruz
        // fantezi olarak
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        virtual public DateTime UpdateDate { get; set; } // tüm entitylerde olmayabilir ovveride edebilrizi
        





    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Exceptions
{
    public class UserCreateFailException : Exception
    {
        public UserCreateFailException():base("Kullanıcı oluşturulurken beklenmedik bir hata oluştu")
        {

        }

        public UserCreateFailException(string? message) : base(message)
        {
        }

        public UserCreateFailException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}

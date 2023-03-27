using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Exceptions
{
    public class AuthenticationErrorExeption : Exception
    {
        public AuthenticationErrorExeption() : base("Kimlik doğrulama hatası")
        {
        }

        public AuthenticationErrorExeption(string? message) : base(message)
        {
        }

        public AuthenticationErrorExeption(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}

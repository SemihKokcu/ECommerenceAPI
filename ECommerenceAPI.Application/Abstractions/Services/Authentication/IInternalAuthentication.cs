using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Abstractions.Services.Authentication
{
    public interface IInternalAuthentication
    {
        Task<DTOs.Token> LoginAsync(string UserNameOrEmail,string password,int tokenLifeTime);
    }
}

﻿using ECommerenceAPI.Application.Abstractions.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Abstractions.Services
{
    public interface IAuthService : IExternalAuthentication,IInternalAuthentication
    {
        
    }
}

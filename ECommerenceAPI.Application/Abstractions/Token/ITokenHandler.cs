﻿using ECommerenceAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T= ECommerenceAPI.Application.DTOs;
namespace ECommerenceAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        T.Token CreateAccessToken(int second,AppUser user);

        string CreateRefreshToken();

    }
}

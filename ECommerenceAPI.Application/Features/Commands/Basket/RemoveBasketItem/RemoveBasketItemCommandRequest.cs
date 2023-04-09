﻿using MediatR;

namespace ECommerenceAPI.Application.Features.Commands.Basket.RemoveBasketItem
{
    public class RemoveBasketItemCommandRequest :IRequest<RemoveBasketItemCommandResponse>
    {
        public string BasketItemId { get; set; }
    }
}
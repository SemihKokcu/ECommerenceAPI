using ECommerenceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Features.Commands.Basket.UpdateQuantityBasketItem
{
    public class UpdateQuantityBasketItemCommandHandler : IRequestHandler<UpdateQuantityBasketItemCommandRequest, UpdateQuantityBasketItemCommandResponse>
    {
        readonly IBasketService _basketService;

        public UpdateQuantityBasketItemCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async  Task<UpdateQuantityBasketItemCommandResponse> Handle(UpdateQuantityBasketItemCommandRequest request, CancellationToken cancellationToken)
        {
            await  _basketService.UpdateQuantityAsync(new()
            {
                 BusketItemId = request.BasketItemId,
                 Quantity = request.Quantity
            });
            return new();
        }
    }
}

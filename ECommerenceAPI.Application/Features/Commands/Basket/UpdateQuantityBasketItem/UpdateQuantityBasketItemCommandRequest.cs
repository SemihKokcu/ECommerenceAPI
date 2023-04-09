using MediatR;

namespace ECommerenceAPI.Application.Features.Commands.Basket.UpdateQuantityBasketItem
{
    public class UpdateQuantityBasketItemCommandRequest :IRequest<UpdateQuantityBasketItemCommandResponse>
    {
        public string BasketItemId { get; set; }
        public int  Quantity { get; set; }
    }
}
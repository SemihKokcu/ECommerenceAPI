using ECommerenceAPI.Application.Abstractions.Services;
using ECommerenceAPI.Application.DTOs.Order;
using ECommerenceAPI.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Persistance.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            var orderCode = (new Random().NextDouble() * 10000).ToString();
            orderCode = orderCode.Substring(orderCode.IndexOf('.')+1,orderCode.Length-orderCode.IndexOf(".")-1);
            await _orderWriteRepository.AddAsync(new()
            {
                Address = createOrder.Address,
                Id = Guid.Parse(createOrder.BasketId),
                Description = createOrder.Description,
                //TODO control nextduble is uniq
                OrderCode = orderCode,
            });

            await _orderWriteRepository.SaveAsync();
        }

        public async Task<ListOrder> GetAllOrderAsync(int page, int size)
        {
            var query = _orderReadRepository.Table.Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .Include(o => o.Basket)
                .ThenInclude(b => b.BasketItems)
                .ThenInclude(p => p.Product);
               
            var data = query.Skip(page * size).Take(size).OrderBy(o => o.CreateDate);
            //.Take((page*size)..size).OrderBy(o=>o.CreateDate)

            return new()
            {
                TotalOrderCount = await query.CountAsync(),
                Orders = await data.Select(o => new
                {
                    Id = o.Id,
                    CreateDate = o.CreateDate,
                    OrderCode = o.OrderCode,
                    TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                    UserName = o.Basket.User.NameSurname
                }).ToListAsync(),
            };
        }

        public async Task<SingleOrder> GetOrderByIdAsync(string id)
        {
            var data = await  _orderReadRepository.Table
                                .Include(o => o.Basket)
                                .ThenInclude(b => b.BasketItems)
                                .ThenInclude(p => p.Product)
                                .FirstOrDefaultAsync(o=>o.Id==Guid.Parse(id));

            return new()
            {
               Id=data.Id.ToString(),
               BasketItems = data.Basket.BasketItems.Select(bi => new
               {
                   bi.Product.Name,
                   bi.Product.Price,
                   bi.Quantity
               }),
               Address = data.Address,
               CreatedDate = data.CreateDate,
               OrderCode = data.OrderCode,
               Description  = data.Description
               
            };

        }
    }
}

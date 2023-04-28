using ECommerenceAPI.Application.Abstractions.Services;
using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerenceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        readonly private IOrderWriteRepository _orderWriteRepository;
        readonly private ICustomerWriteRepository _customerWriteRepository;
        readonly private IOrderReadRepository _orderReadRepository;
        readonly private IMailService _mailService;
        public TestsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IOrderWriteRepository orderWriteRepository, ICustomerWriteRepository customerWriteRepository, IOrderReadRepository orderReadRepository, IMailService mailService)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _customerWriteRepository = customerWriteRepository;
            _orderReadRepository = orderReadRepository;
            _mailService = mailService;
        }
        [HttpGet]
        // bu metot tipi Task yani async bir tip olmadığından Ioc containerda 
        // nesenmiz Scoped olarak yaratılır ve burada bu metot async beklemediği için
        // nesne daha işini yapamadan imha edilir
        public async Task<IActionResult> Get() 
        {
            //await _productWriteRepository.AddRangeAsync(new()
            //{
            //    new() { Id = Guid.NewGuid(), Name = "Product1", CreateDate = DateTime.UtcNow, Stock = 10 },
            //    new() { Id = Guid.NewGuid(), Name = "Product2", CreateDate = DateTime.UtcNow, Stock = 10 },
            //    new() { Id = Guid.NewGuid(), Name = "Product3", CreateDate = DateTime.UtcNow, Stock = 10 },

            //});
            // await _productWriteRepository.SaveAsync();

            //scoped kullandığımız için read ve write için de aynı instance elde eidlir o yüzden write savechanges kullanılabilir
            //tracking true iken değişikliker uygulanır fakat false iken uygulanmaz çünkü veri takip edilmez
            //Product product = await _productReadRepository.GetByIdAsync("14717b25-b30e-474e-9582-de429e05a7a5",false);
            //product.Name = "Mehmet";
            //await _productWriteRepository.SaveAsync();
            //var customerId = Guid.NewGuid();
            //_customerWriteRepository.AddAsync(new() { Name = "semih",Id=customerId  });
            //_orderWriteRepository.AddAsync(new() { Description = "bla bla", Address = "Bursa", CustomerId = customerId });
            //_orderWriteRepository.AddAsync(new() { Description = "bla bla 2", Address = "Zong", CustomerId=customerId });
            //await _orderWriteRepository.SaveAsync();

            //Order order =  await _orderReadRepository.GetByIdAsync("51f79047-daa0-423a-a543-a829489b37ef");
            //order.Address = "İstanbul";
            //await _orderWriteRepository.SaveAsync();

            return Ok("merhaba");

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Product product =   await _productReadRepository.GetByIdAsync(id);
            return Ok(product);

        }

      
    }
}

using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerenceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;

        public ProductController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }
        [HttpGet]
        // bu metot tipi Task yani async bir tip olmadığından Ioc containerda 
        // nesenmiz Scoped olarak yaratılır ve burada bu metot async beklemediği için
        // nesne daha işini yapamadan imha edilir
        public async Task Get() 
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
            Product product = await _productReadRepository.GetByIdAsync("14717b25-b30e-474e-9582-de429e05a7a5",false);
            product.Name = "Mehmet";
            await _productWriteRepository.SaveAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Product product =   await _productReadRepository.GetByIdAsync(id);
            return Ok(product);

        }
    }
}

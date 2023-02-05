using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Application.RequestParametres;
using ECommerenceAPI.Application.ViewModels.Products;
using ECommerenceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerenceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;


        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        [HttpGet]                               //query den gönderdik
        public async Task<IActionResult> GetAll([FromQuery]Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products= _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreateDate,
                p.UpdateDate
            });
            return Ok(new { totalCount,products }); // veri get ederken trackinge gerek yok fasle verdik
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id,false));
        }

        //[HttpPost(Name ="add")] // gelen neseneyi entity işe karşışamak doğru değildir viewmodeller oluşturup karşılaiırız. CQRS pattern da request ile alıcaz 
        //public async Task<IActionResult> Add(VM_Create_Product model)
        //{

        //    await _productWriteRepository.AddAsync(new()
        //    {
        //        Name = model.Name,
        //        Price= model.Price,
        //        Stock=model.Stock,
        //    });
        //    await _productWriteRepository.SaveAsync();
        //    //return Ok();
        //    return Ok();
        //}
        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            
            await _productWriteRepository.AddAsync(new Product()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Name = model.Name;
            product.Price = model.Price;
            //gelen veri track edildiği için ef nin sağlamış olduğu update fonksiyonunu kullanmamzıa gerek yoktur.
            //Burada saveChanges edildiğinde update olacaktır
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }

}

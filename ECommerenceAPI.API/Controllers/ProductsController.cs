using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Application.RequestParametres;
using ECommerenceAPI.Application.Services;
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
        readonly private IWebHostEnvironment _webHostEnvironment;
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        readonly IFileService _fileService;
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IFileReadRepository _fileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IProductImageFileReadRepository _productImageImageFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        public ProductsController(IWebHostEnvironment webHostEnvironment, IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IFileService fileService, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageFileReadRepository productImageImageFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _fileService = fileService;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageImageFileReadRepository = productImageImageFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
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

        //action girmeliyiz artık bir tane post olduğundan artık aciton adı girmek zorundayız
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {


            var datas = await _fileService.UploadAsync("resource/files", Request.Form.Files);
            //await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile() 
            //{ 
            //    FileName = d.fileName,
            //    Path = d.path,
            //}).ToList());
            //await _productImageFileWriteRepository.SaveAsync();

            //await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //    Price = new Random().Next()
            //}).ToList());
            //await _invoiceFileWriteRepository.SaveAsync();

            await _fileWriteRepository.AddRangeAsync(datas.Select(d => new ECommerenceAPI.Domain.Entities.File()
            {
                FileName = d.fileName,
                Path = d.path,
            }).ToList());
            await _fileWriteRepository.SaveAsync();

            //var d1 = _fileReadRepository.GetAll(false);
            //var d2 = _invoiceFileReadRepository.GetAll(false);
            //var d3 = _productImageImageFileReadRepository.GetAll(false);


            return Ok();












            ////wwwroot/resource/product-image
            //string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");

            //if (!Directory.Exists(uploadPath))
            //{
            //    Directory.CreateDirectory(uploadPath);
            //}
            //Random random = new Random();
            //foreach (IFormFile file in Request.Form.Files) 
            //{
            //    string fullPath = Path.Combine(uploadPath, $"{random.Next()}{Path.GetExtension(file.FileName)}");
            //    using FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None,1024*1024,useAsync:false);

            //    await file.CopyToAsync(fileStream);
            //    await fileStream.FlushAsync();

            //    //dosya kaydı için path aldık rastegel isim verdik filestram kullamdık ve sonra onu serbest bıraktık

            //}
            //return Ok();
        }
    }

}

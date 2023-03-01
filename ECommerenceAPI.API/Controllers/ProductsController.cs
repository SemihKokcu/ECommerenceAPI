using ECommerenceAPI.Application.Abstractions.Storage;
using ECommerenceAPI.Application.Features.Commands.CreateProduct;
using ECommerenceAPI.Application.Features.Queries.GetAllProduct;
using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Application.RequestParametres;
using ECommerenceAPI.Application.ViewModels.Products;
using ECommerenceAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IFileReadRepository _fileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IProductImageFileReadRepository _productImageImageFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration _configuration;

        readonly IMediator _mediator;
        public ProductsController(IWebHostEnvironment webHostEnvironment, IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageFileReadRepository productImageImageFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator)
        {
            _webHostEnvironment = webHostEnvironment;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageImageFileReadRepository = productImageImageFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpGet]                               //query den gönderdik
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id,false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
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
        [HttpPost("[action]")] // routte parametre bildirmedik querystring de aldık
        public async Task<IActionResult> Upload(string id)
        {


            //var datas = await _storageService.UploadAsync("files", Request.Form.Files);
            ////await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile() 
            ////{ 
            ////    FileName = d.fileName,
            ////    Path = d.path,
            ////}).ToList());
            ////await _productImageFileWriteRepository.SaveAsync();

            ////await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            ////{
            ////    FileName = d.fileName,
            ////    Path = d.path,
            ////    Price = new Random().Next()
            ////}).ToList());
            ////await _invoiceFileWriteRepository.SaveAsync();

            //await _fileWriteRepository.AddRangeAsync(datas.Select(d => new ECommerenceAPI.Domain.Entities.File()
            //{
            //    FileName = d.fileName,
            //    Path = d.pathOrContainerName,
            //    Storage = _storageService.StroageName
            //}).ToList());
            //await _fileWriteRepository.SaveAsync();

            ////var d1 = _fileReadRepository.GetAll(false);
            ////var d2 = _invoiceFileReadRepository.GetAll(false);
            ////var d3 = _productImageImageFileReadRepository.GetAll(false);
            ///


           List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photos-images", Request.Form.Files);
           
            
            
            Product product = await _productReadRepository.GetByIdAsync(id);

            //foreach (var r in result)
            //{
            //    product.ProductImageFiles.Add(new()
            //    {
            //        FileName = r.fileName,
            //        Path = r.pathOrContainerName,
            //        Storage = _storageService.StroageName,
            //        Products = new List<Product>() { product }
            //    });
            //}

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r=> new ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StroageName,
                Products = new List<Product>() { product}
                
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();


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

        [HttpGet("[action]/{id}")] 
        public async Task<IActionResult> GetProductImage(string id)
        {
            //join olduk
            Product? product = await  _productReadRepository.Table.Include(p=>p.ProductImageFiles)
                .FirstOrDefaultAsync(p=>p.Id == Guid.Parse(id));
            return Ok(product.ProductImageFiles.Select(p => new
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id,string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
               .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            ProductImageFile? productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }

}

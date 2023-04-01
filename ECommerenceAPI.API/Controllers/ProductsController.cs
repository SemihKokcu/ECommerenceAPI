using ECommerenceAPI.Application.Abstractions.Storage;
using ECommerenceAPI.Application.Features.Commands.Product.CreateProduct;
using ECommerenceAPI.Application.Features.Commands.Product.RemoveProduct;
using ECommerenceAPI.Application.Features.Commands.Product.UpdateProduct;
using ECommerenceAPI.Application.Features.Commands.ProductImageFile.ChangeShowCaseImage;
using ECommerenceAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ECommerenceAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ECommerenceAPI.Application.Features.Queries.Product.GetAllProduct;
using ECommerenceAPI.Application.Features.Queries.Product.GetByIdProduct;
using ECommerenceAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using ECommerenceAPI.Application.Repositories;
using ECommerenceAPI.Application.RequestParametres;
using ECommerenceAPI.Application.ViewModels.Products;
using ECommerenceAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerenceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]                               //query den gönderdik
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }

        


        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest )
        {
            RemoveProductCommandResponse response =  await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }

        //action girmeliyiz artık bir tane post olduğundan artık aciton adı girmek zorundayız
        [HttpPost("[action]")] // routte parametre bildirmedik querystring de aldık
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response =  await _mediator.Send(uploadProductImageCommandRequest);
            return Ok();

        }

        [HttpGet("[action]/{id}")] 
        public async Task<IActionResult> GetProductImage([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest )
        {
            List<GetProductImagesQueryResponse> response = await _mediator.Send(getProductImagesQueryRequest);
            return Ok(response);
           
        }

        [HttpDelete("[action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest,[FromQuery]string imageId)
        {
            removeProductImageCommandRequest.ImageId = imageId;
            RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery] ChangeShowCaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
            ChangeShowCaseImageCommandResponse response = await _mediator.Send(changeShowcaseImageCommandRequest);
            return Ok(response);
        }
    }

}

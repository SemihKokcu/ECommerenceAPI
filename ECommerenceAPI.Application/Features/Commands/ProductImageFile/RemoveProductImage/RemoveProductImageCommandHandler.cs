using ECommerenceAPI.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P = ECommerenceAPI.Domain.Entities;

namespace ECommerenceAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandHandler : MediatR.IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;

        public RemoveProductImageCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            P.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
               .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

            P.ProductImageFile? productImageFile = product?.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(request.ImageId));

            if (productImageFile !=null)
                product?.ProductImageFiles.Remove(productImageFile);
            
            await _productWriteRepository.SaveAsync();
            return new();

        }
    }
}

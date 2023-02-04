using ECommerenceAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Application.Validators
{
    public  class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Ürün adı boş olamaz")
             .MaximumLength(150)
                .MinimumLength(2)
                    .WithMessage("Ürün adı 2-50 karakter arasında olmalıdır");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Stock miktarı boş olamaz")
                .Must(s => s >= 0)
                    .WithMessage("Stock bilgisi 0 dan büyük olmalıdır.");

            RuleFor(p => p.Price)
               .NotEmpty()
               .NotNull()
                   .WithMessage("Fiyat bilgisi boş olamaz")
               .Must(s => s >= 0)
                   .WithMessage("Fiyat bilgisi 0 dan büyük olmalıdır.");
        }

    }
}

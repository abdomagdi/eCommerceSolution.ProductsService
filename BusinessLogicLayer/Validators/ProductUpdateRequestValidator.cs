using BusinessLogicLayer.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Validators
{
    public class ProductUpdateRequestValidator:AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator()
        {
            RuleFor(x => x.ProductID).NotEmpty().WithMessage("ProductID is required.");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name is required.")
               .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");
            RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than zero.");
            RuleFor(x => x.QuantityInStock).GreaterThanOrEqualTo(0).WithMessage("Quantity in stock cannot be negative.");
            RuleFor(x => x.Category).IsInEnum().WithMessage("Invalid category.");
        }
    }
}

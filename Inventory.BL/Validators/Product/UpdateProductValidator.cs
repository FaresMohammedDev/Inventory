using FluentValidation;
using Inventory.BL.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Validators.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is requird")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 character");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is requird")
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot less than 0");

            RuleFor(x => x.StockQuantity)
                .NotEmpty().WithMessage("Stock quantity is requird")
                .GreaterThanOrEqualTo(-1).WithMessage("Quantity mustn't be negative number");
        }
    }
}

using FluentValidation;
using Inventory.BL.DTOs.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Validators.OrderItem
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(x => x.UnitPrice)
                .NotEmpty().WithMessage("Price is requird")
                .GreaterThanOrEqualTo(0).WithMessage("Price must greater than 0");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is requird")
                .GreaterThanOrEqualTo(-1).WithMessage("Quantity mustn't be negative number");
        }
    }
}

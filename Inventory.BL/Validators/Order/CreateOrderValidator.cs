using FluentValidation;
using Inventory.BL.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Validators.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Order Date is requird");

            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("Total Price is requird")
                .GreaterThanOrEqualTo(0).WithMessage("Total price must greater than 0");
        }
    }
}

using Inventory.BL.DTOs.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.DTOs.Order
{
    public class PlaceOrderRequest
    {
        public CreateOrderDto Order { get; set; } = null!;
        public List<OrderItemRequestDto> Items { get; set; } = new();
    }
}

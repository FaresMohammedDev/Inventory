using Inventory.BL.Common;
using Inventory.BL.DTOs.Order;
using Inventory.BL.DTOs.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResponse<IEnumerable<GetOrderDto>>> GetAllOrdersAsync();
        Task<ServiceResponse<GetOrderDto>> GetOrderByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<GetOrderDto>>> GetOrdersByUserIdAsync(int userId);
        Task<ServiceResponse<int>> CreateOrderAsync(CreateOrderDto orderDto, List<OrderItemRequestDto> itemsDto);
        Task<ServiceResponse<string>> CancelOrderAsync(int orderId);
    }
}
    
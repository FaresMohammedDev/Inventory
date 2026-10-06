using Inventory.BL.Common;
using Inventory.BL.DTOs.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<ServiceResponse<IEnumerable<GetOrderItemDto>>> GetAllAsync();
        Task<ServiceResponse<GetOrderItemDto>> GetByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<GetOrderItemDto>>> GetItemsByOrderIdAsync(int orderId);
        Task<ServiceResponse<string>> AddItemToOrderAsync(CreateOrderItemDto dto);
        Task<ServiceResponse<string>> UpdateItemQuantityAsync(int itemId, UpdateOrderItemDto dto);
        Task<ServiceResponse<string>> RemoveItemFromOrderAsync(int itemId);
    }
}

using Inventory.BL.DTOs.OrderItem;
using Inventory.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var response = await _orderItemService.GetAllAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _orderItemService.GetByIdAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var response = await _orderItemService.GetItemsByOrderIdAsync(orderId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddItemToOrder([FromBody] CreateOrderItemDto dto)
        {
            var response = await _orderItemService.AddItemToOrderAsync(dto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}/quantity")]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int id, [FromBody] UpdateOrderItemDto dto)
        {
            var response = await _orderItemService.UpdateItemQuantityAsync(id, dto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var response = await _orderItemService.RemoveItemFromOrderAsync(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}

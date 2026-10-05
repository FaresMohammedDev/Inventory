using Inventory.BL.Common;
using Inventory.BL.DTOs.OrderItem;
using Inventory.BL.Services.Interfaces;
using Inventory.DAL.Models;
using Inventory.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services.Implementation
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IProductRepo _productRepo;

        public OrderItemService(IOrderItemRepo orderItemRepo, IOrderRepo orderRepo, IProductRepo productRepo)
        {
            _orderItemRepo = orderItemRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        public async Task<ServiceResponse<IEnumerable<GetOrderItemDto>>> GetAllAsync()
        {
            var items = await _orderItemRepo.GetAllAsync();
            var dtos = items.Select(i => new GetOrderItemDto
            {
                Id = i.Id,
                OrderId = i.OrderId,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            return ServiceResponse<IEnumerable<GetOrderItemDto>>.Success(dtos);
        }

        public async Task<ServiceResponse<GetOrderItemDto>> GetByIdAsync(int id)
        {
            var item = await _orderItemRepo.GetByIdAsync(id);
            if (item == null)
                return ServiceResponse<GetOrderItemDto>.Fail("Order Item not found.");

            var dto = new GetOrderItemDto
            {
                Id = item.Id,
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            };

            return ServiceResponse<GetOrderItemDto>.Success(dto);
        }

        public async Task<ServiceResponse<IEnumerable<GetOrderItemDto>>> GetItemsByOrderIdAsync(int orderId)
        {
            var allItems = await _orderItemRepo.GetAllAsync();
            var dtos = allItems.Where(i => i.OrderId == orderId).Select(i => new GetOrderItemDto
            {
                Id = i.Id,
                OrderId = i.OrderId,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            return ServiceResponse<IEnumerable<GetOrderItemDto>>.Success(dtos);
        }

        public async Task<ServiceResponse<string>> AddItemToOrderAsync(CreateOrderItemDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(dto.OrderId);
            if (order == null) return ServiceResponse<string>.Fail("Order not found.");

            var product = await _productRepo.GetByIdAsync(dto.ProductId);
            if (product == null) return ServiceResponse<string>.Fail("Product not found.");

            if (product.StockQuantity < dto.Quantity)
                return ServiceResponse<string>.Fail($"Not enough stock. Available: {product.StockQuantity}");

            product.StockQuantity -= dto.Quantity;
            await _productRepo.UpdateAsync(product);
            await _productRepo.SaveChangesAsync();

            var orderItem = new OrderItem
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = product.Price
            };
            await _orderItemRepo.CreateAsync(orderItem);
            await _orderItemRepo.SaveChangesAsync();

            order.TotalPrice += (product.Price * dto.Quantity);
            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();

            return ServiceResponse<string>.Success(string.Empty, "Item added to order, stock deducted, and total price updated.");
        }

        public async Task<ServiceResponse<string>> UpdateItemQuantityAsync(int itemId, UpdateOrderItemDto dto)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(itemId);
            if (orderItem == null) return ServiceResponse<string>.Fail("Order Item not found.");

            var product = await _productRepo.GetByIdAsync(orderItem.ProductId);
            var order = await _orderRepo.GetByIdAsync(orderItem.OrderId);

            int quantityDifference = dto.Quantity - orderItem.Quantity;

            if (quantityDifference > 0)
            {
                if (product.StockQuantity < quantityDifference)
                    return ServiceResponse<string>.Fail($"Not enough stock to add {quantityDifference} more.");

                product.StockQuantity -= quantityDifference;
                order.TotalPrice += (orderItem.UnitPrice * quantityDifference);
            }
            else if (quantityDifference < 0)
            {
                int amountToReturn = Math.Abs(quantityDifference);
                product.StockQuantity += amountToReturn;
                order.TotalPrice -= (orderItem.UnitPrice * amountToReturn);
            }

            orderItem.Quantity = dto.Quantity;

            await _productRepo.UpdateAsync(product);
            await _productRepo.SaveChangesAsync();

            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();

            await _orderItemRepo.UpdateAsync(orderItem);
            await _orderItemRepo.SaveChangesAsync();

            return ServiceResponse<string>.Success(string.Empty, "Order Item updated securely.");
        }

        public async Task<ServiceResponse<string>> RemoveItemFromOrderAsync(int itemId)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(itemId);
            if (orderItem == null) return ServiceResponse<string>.Fail("Order Item not found.");

            var product = await _productRepo.GetByIdAsync(orderItem.ProductId);
            var order = await _orderRepo.GetByIdAsync(orderItem.OrderId);

            if (product != null)
            {
                product.StockQuantity += orderItem.Quantity;
                await _productRepo.UpdateAsync(product);
                await _productRepo.SaveChangesAsync();
            }

            if (order != null)
            {
                order.TotalPrice -= (orderItem.UnitPrice * orderItem.Quantity);
                await _orderRepo.UpdateAsync(order);
                await _productRepo.SaveChangesAsync();
            }

            await _orderItemRepo.DeleteAsync(orderItem);
            await _productRepo.SaveChangesAsync();

            return ServiceResponse<string>.Success(string.Empty, "Item removed, stock returned, and order price adjusted.");
        }
    }
}

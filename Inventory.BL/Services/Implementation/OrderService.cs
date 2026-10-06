using Inventory.BL.Common;
using Inventory.BL.DTOs.Order;
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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IProductRepo _productRepo;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepo orderRepo, IOrderItemRepo orderItemRepo, IProductRepo productRepo, IUnitOfWork unitOfWork)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<IEnumerable<GetOrderDto>>> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            var orderDtos = orders.Select(o => new GetOrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                UserName = o.User != null ? o.User.FullName : "Unknown"
            }).ToList();

            return ServiceResponse<IEnumerable<GetOrderDto>>.Success(orderDtos, "Orders retrieved successfully");
        }

        public async Task<ServiceResponse<GetOrderDto>> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
                return ServiceResponse<GetOrderDto>.Fail("Order not found!");

            var orderDto = new GetOrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                UserName = order.User != null ? order.User.FullName : "Unknown"
            };

            return ServiceResponse<GetOrderDto>.Success(orderDto);
        }

        public async Task<ServiceResponse<IEnumerable<GetOrderDto>>> GetOrdersByUserIdAsync(int userId)
        {
            var allOrders = await _orderRepo.FindAsync(x => x.UserId == userId);

            var userOrders = allOrders
                .Select(o => new GetOrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    UserName = o.User != null ? o.User.FullName : "Unknown"
                }).ToList();

            return ServiceResponse<IEnumerable<GetOrderDto>>.Success(userOrders);
        }

        public async Task<ServiceResponse<int>> CreateOrderAsync(CreateOrderDto orderDto, List<OrderItemRequestDto> itemsDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                decimal calculatedTotalPrice = 0;
                foreach (var item in itemsDto)
                {
                    var product = await _productRepo.GetByIdAsync(item.ProductId);
                    if (product == null)
                        return ServiceResponse<int>.Fail($"Product ID {item.ProductId} not found.");

                    if (product.StockQuantity < item.Quantity)
                        return ServiceResponse<int>.Fail($"Not enough stock for {product.Name}. Available: {product.StockQuantity}");

                    calculatedTotalPrice += product.Price * item.Quantity;
                }

                var newOrder = new Order
                {
                    OrderDate = orderDto.OrderDate,
                    TotalPrice = calculatedTotalPrice,
                    UserId = orderDto.UserId
                };
                await _orderRepo.CreateAsync(newOrder);
                await _orderItemRepo.SaveChangesAsync();

                foreach (var itemDto in itemsDto)
                {
                    var product = await _productRepo.GetByIdAsync(itemDto.ProductId);
                    product.StockQuantity -= itemDto.Quantity;
                    await _productRepo.UpdateAsync(product);
                    await _productRepo.SaveChangesAsync();

                    var orderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = product.Price
                    };
                    await _orderItemRepo.CreateAsync(orderItem);
                    await _orderItemRepo.SaveChangesAsync();
                }

                await _unitOfWork.CommitAsync();
                return ServiceResponse<int>.Success(newOrder.Id, "Order created successfully and stock deducted.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ServiceResponse<int>.Fail($"Order creation failed: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<string>> CancelOrderAsync(int orderId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var order = await _orderRepo.GetByIdAsync(orderId);
                if (order == null)
                    return ServiceResponse<string>.Fail("Order not found.");

                var orderItems = await _orderItemRepo.FindAsync(x => x.OrderId == orderId);

                foreach (var item in orderItems)
                {
                    var product = await _productRepo.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += item.Quantity;
                        await _productRepo.UpdateAsync(product);
                        await _productRepo.SaveChangesAsync();
                    }

                    await _orderItemRepo.DeleteAsync(item);
                    await _orderItemRepo.SaveChangesAsync();
                }

                await _orderRepo.DeleteAsync(order);
                await _orderRepo.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
                return ServiceResponse<string>.Success(string.Empty, "Order cancelled and stock returned successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return ServiceResponse<string>.Fail($"Order cancellation failed: {ex.Message}");
            }
        }
    }
}
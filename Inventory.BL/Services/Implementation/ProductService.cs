using Inventory.BL.Common;
using Inventory.BL.DTOs.Product;
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
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;

        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ServiceResponse<IEnumerable<GetProductDto>>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var productDtos = products.Select(p => new GetProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            }).ToList();

            return ServiceResponse<IEnumerable<GetProductDto>>.Success(productDtos, "Products retrieved successfully.");
        }

        public async Task<ServiceResponse<GetProductDto>> GetProductByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return ServiceResponse<GetProductDto>.Fail("Product not found!");

            var productDto = new GetProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            return ServiceResponse<GetProductDto>.Success(productDto);
        }

        public async Task<ServiceResponse<int>> CreateProductAsync(CreateProductDto dto)
        {
            if (dto.Price <= 0)
                return ServiceResponse<int>.Fail("Price must be greater than zero.");

            if (dto.StockQuantity < 0)
                return ServiceResponse<int>.Fail("Stock quantity cannot be negative.");

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity
            };

            await _productRepo.CreateAsync(product);
            await _productRepo.SaveChangesAsync();
            return ServiceResponse<int>.Success(product.Id, "Product created successfully.");
        }

        public async Task<ServiceResponse<int>> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            if (dto.Price <= 0)
                return ServiceResponse<int>.Fail("Price must be greater than zero.");

            if (dto.StockQuantity < 0)
                return ServiceResponse<int>.Fail("Stock quantity cannot be negative.");

            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return ServiceResponse<int>.Fail("Product not found!");

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;

            await _productRepo.UpdateAsync(product);
            await _productRepo.SaveChangesAsync();

            return ServiceResponse<int>.Success(product.Id, "Product updated successfully.");
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return ServiceResponse<bool>.Fail("Product not found!");

            await _productRepo.DeleteAsync(product);
            await _productRepo.SaveChangesAsync();

            return ServiceResponse<bool>.Success(true, "Product deleted successfully.");
        }
    }
}

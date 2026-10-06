using Inventory.BL.Common;
using Inventory.BL.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<IEnumerable<GetProductDto>>> GetAllProductsAsync();
        Task<ServiceResponse<GetProductDto>> GetProductByIdAsync(int id);
        Task<ServiceResponse<int>> CreateProductAsync(CreateProductDto dto);
        Task<ServiceResponse<int>> UpdateProductAsync(int id, UpdateProductDto dto);
        Task<ServiceResponse<bool>> DeleteProductAsync(int id);
    }
}

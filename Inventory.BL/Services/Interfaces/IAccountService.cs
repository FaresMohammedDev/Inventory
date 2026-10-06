using Inventory.BL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.BL.DTOs.Account;

namespace Inventory.BL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResponse<string>> RegisterAsync(RegisterDto dto);
        Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}

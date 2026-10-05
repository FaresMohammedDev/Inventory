using Inventory.BL.Common;
using Inventory.BL.DTOs.Account;
using Inventory.BL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services.Implementation
{
    public class AccountService : IAccountService
    {
        public Task<ServiceResponse<string>> LoginAsync(LoginDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<string>> RegisterAsync(RegisterDto dto)
        {
            throw new NotImplementedException();
        }
    }
}

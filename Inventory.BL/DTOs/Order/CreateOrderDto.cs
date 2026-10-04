using Inventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.DTOs.Order
{
    public record CreateOrderDto
    {
        public DateTime OrderDate { get; set; }
        public int TotalPrice { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}

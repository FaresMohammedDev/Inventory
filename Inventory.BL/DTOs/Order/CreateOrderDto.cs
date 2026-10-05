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
        public int UserId { get; set; }
    }
}

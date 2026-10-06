using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}

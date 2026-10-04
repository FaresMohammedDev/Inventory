using Inventory.DAL.Data;
using Inventory.DAL.Models;
using Inventory.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Repositories.Implementation
{
    public class OrderItemRepo : GenericRepo<OrderItem>, IOrderItemRepo
    {
        public OrderItemRepo(ApplicationDbContext context, DbSet<OrderItem> dbSet) : base(context, dbSet)
        {

        }
    }
}

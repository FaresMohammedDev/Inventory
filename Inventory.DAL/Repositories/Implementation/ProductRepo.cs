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
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public ProductRepo(ApplicationDbContext context) : base(context)
        {

        }
    }
}

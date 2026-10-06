using Inventory.BL.Services.Interfaces;
using Inventory.DAL.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitAsync()
        {
            try { await _transaction.CommitAsync(); }
            finally { await _transaction.DisposeAsync(); }
        }

        public async Task RollbackAsync()
        {
            try { await _transaction.RollbackAsync(); }
            finally { await _transaction.DisposeAsync(); }
        }
    }
}

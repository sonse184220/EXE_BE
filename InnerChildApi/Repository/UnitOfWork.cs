using Microsoft.EntityFrameworkCore.Storage;
using Repository.DBContext;
using Repository.Repositories;

namespace Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ITransactionRepository TransactionRepository { get; }
        IPurchaseRepository PurchaseRepository { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InnerChildExeContext _context;
        private IDbContextTransaction _transaction;

        public ITransactionRepository TransactionRepository { get; }

        public IPurchaseRepository PurchaseRepository { get; }

        public UnitOfWork()
        {
            _context ??= new InnerChildExeContext();
            TransactionRepository = new TransactionRepository(_context);
            PurchaseRepository = new PurchaseRepository(_context);
        }


        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface ITransactionRepository
    {
        Task<int> CreateTransactionAsync(Transaction transaction);
        Task<int> UpdateTransactionAsync(Transaction transaction);
        Task<Transaction> GetTransactionByIdAsync(string transactionId);
    }
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository()
        {

        }
        public TransactionRepository(InnerChildExeContext context) : base(context)
        {

        }
        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            return await _context.Transactions.Include(x => x.Subscription).Include(x => x.User).ThenInclude(x => x.Profiles).FirstOrDefaultAsync(x => x.TransactionId == transactionId);
        }
        public async Task<int> CreateTransactionAsync(Transaction transaction)
        {
            return await CreateAsync(transaction);
        }

        public async Task<int> UpdateTransactionAsync(Transaction transaction)
        {
            return await UpdateAsync(transaction);
        }
        public void PrepareUpdateTransaction(Transaction transaction)
        {
            PrepareUpdate(transaction);
        }
    }
}

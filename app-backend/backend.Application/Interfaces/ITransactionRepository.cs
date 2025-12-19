using Backend.Domain.Entities;

namespace Backend.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction transaction);
        Task<Transaction?> GetByIdAsync(Guid transactionId);
        Task<List<Transaction>> GetAllAsync();
        Task<List<Transaction>> GetAllForAccountAsync(Guid accountId);
        Task<int> GetTotalCountAsync();
    }
}

using Backend.Domain.Entities;

namespace Backend.Application.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(Guid accountId, int amount);
        Task<Transaction?> GetTransactionByIdAsync(Guid transactionId);
        Task<List<Transaction>> GetAllTransactionsAsync();
    }
}

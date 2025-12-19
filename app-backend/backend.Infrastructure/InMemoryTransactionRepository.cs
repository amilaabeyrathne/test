using Backend.Domain.Entities;
using Backend.Domain.Exceptions;
using Backend.Application.Interfaces;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

namespace Backend.Infrastructure
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        private readonly int MaxTransactionLimit;
        private readonly ConcurrentDictionary<Guid, Transaction> _transactions = new();
        private readonly object _lock = new object();

        public InMemoryTransactionRepository(IConfiguration configuration)
        {
            MaxTransactionLimit = configuration.GetValue<int>("MaxTransactionLimit", 100000);
        }

        public Task<Transaction> AddAsync(Transaction transaction)
        {
            lock (_lock) // even though ConcurrentDictionary is thread-safe, we need to check the count and add atomically
            {
                if (_transactions.Count >= MaxTransactionLimit)
                {
                    throw new TransactionLimitExceededException(MaxTransactionLimit);
                }

                if (!_transactions.TryAdd(transaction.TransactionId, transaction))
                {
                    throw new TransactionAlreadyExistException(transaction.TransactionId);
                }
            }

            return Task.FromResult(transaction);
        }

        public Task<Transaction?> GetByIdAsync(Guid transactionId)
        {
            _transactions.TryGetValue(transactionId, out var transaction);
            return Task.FromResult(transaction);
        }

        public Task<List<Transaction>> GetAllAsync()
        {
            var allTransactions = _transactions.Values
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            return Task.FromResult(allTransactions);
        }

        public Task<List<Transaction>> GetAllForAccountAsync(Guid accountId)
        {
            var accountTransactions = _transactions.Values
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.CreatedAt).ToList();

            return Task.FromResult(accountTransactions);
        }

        public Task<int> GetTotalCountAsync()
        {
            return Task.FromResult(_transactions.Count);
        }
    }
}

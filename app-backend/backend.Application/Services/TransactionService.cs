using Backend.Domain.Entities;
using Backend.Application.Interfaces;
using Backend.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(
       ITransactionRepository transactionRepository,
       ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task<Transaction> CreateTransactionAsync(Guid accountId, int amount)
        {
            _logger.LogInformation(
                "Creating transaction for account {AccountId} with amount {Amount}",
                accountId, amount);

            var transaction = new Transaction(accountId, amount);

            var createdTransaction = await _transactionRepository.AddAsync(transaction);

            _logger.LogInformation(
                "Transaction {TransactionId} created successfully for account {AccountId}",
                createdTransaction.TransactionId, accountId);

            return createdTransaction;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(Guid transactionId)
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction {TransactionId} not found", transactionId);
                return null;
            }

            return transaction;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();

            return transactions;
        }
    }
}

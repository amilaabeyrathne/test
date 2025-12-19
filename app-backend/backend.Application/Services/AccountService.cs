using Backend.Application.Interfaces;
using Backend.Application.Services.Interfaces;
using Backend.Domain.Entities;
using Microsoft.Extensions.Logging;


namespace Backend.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            ITransactionRepository transactionRepository,
            ILogger<AccountService> logger)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task<Account?> GetAccountByIdAsync(Guid accountId)
        {
            var transactions = await _transactionRepository.GetAllForAccountAsync(accountId);

            if (!transactions.Any())
            {
                _logger.LogWarning("Account {AccountId} not found (no transactions exist)", accountId);
                return null;
            }

            var balance = transactions.Sum(t => t.Amount);
            var transactionCount = transactions.Count;

            return new Account(accountId, balance);
        }
    }
}

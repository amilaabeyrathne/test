using Backend.Application.Interfaces;
using Backend.Application.Services;
using Backend.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Backend.Tests
{
    public class AccountServiceTests
    {
        private readonly Mock<ILogger<AccountService>> _loggerMock;

        public AccountServiceTests()
        {
            _loggerMock = new Mock<ILogger<AccountService>>();
        }

        [Fact]
        public async Task GetAccountByIdAsync_Should_Return_Account_With_Correct_Balance()
        {
            var accountId = Guid.NewGuid();
            var transactions = new List<Transaction>
        {
            new Transaction(accountId, 100),
            new Transaction(accountId, -30),
            new Transaction(accountId, 50)
        };

            var transactionRepoMock = new Mock<ITransactionRepository>();
            transactionRepoMock
                .Setup(r => r.GetAllForAccountAsync(accountId))
                .ReturnsAsync(transactions);

            var service = new AccountService(transactionRepoMock.Object, _loggerMock.Object);

            var result = await service.GetAccountByIdAsync(accountId);

            Assert.NotNull(result);
            Assert.Equal(accountId, result!.AccountId);
            Assert.Equal(120, result.Balance); // 100 - 30 + 50
            transactionRepoMock.Verify(r => r.GetAllForAccountAsync(accountId), Times.Once);
        }

        [Fact]
        public async Task GetAccountByIdAsync_Should_Return_Null_When_Account_Not_Found()
        {
            var accountId = Guid.NewGuid();
            var emptyTransactions = new List<Transaction>();

            var transactionRepoMock = new Mock<ITransactionRepository>();
            transactionRepoMock
                .Setup(r => r.GetAllForAccountAsync(accountId))
                .ReturnsAsync(emptyTransactions);

            var service = new AccountService(transactionRepoMock.Object, _loggerMock.Object);

            var result = await service.GetAccountByIdAsync(accountId);

            Assert.Null(result);
            transactionRepoMock.Verify(r => r.GetAllForAccountAsync(accountId), Times.Once);
        }
    }
}

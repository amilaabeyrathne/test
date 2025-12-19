using Backend.Application.Interfaces;
using Backend.Application.Services;
using Backend.Domain.Entities;
using Backend.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Backend.Tests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ILogger<TransactionService>> _loggerMock;

        public TransactionServiceTests()
        {
            _loggerMock = new Mock<ILogger<TransactionService>>();
        }

        [Fact]
        public async Task CreateTransactionAsync_Should_Create_Transaction_And_Call_Repository()
        {
            var repoMock = new Mock<ITransactionRepository>();
            repoMock
                .Setup(r => r.AddAsync(It.IsAny<Transaction>()))
                .ReturnsAsync((Transaction t) => t);

            var service = new TransactionService(repoMock.Object, _loggerMock.Object);
            var accountId = Guid.NewGuid();
            var amount = 25;

            var result = await service.CreateTransactionAsync(accountId, amount);

            Assert.Equal(accountId, result.AccountId);
            Assert.Equal(amount, result.Amount);
            Assert.NotEqual(Guid.Empty, result.TransactionId);
            repoMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_Should_Return_Transaction_When_Found()
        {
            var transactionId = Guid.NewGuid();
            var expected = new Transaction(Guid.NewGuid(), 10);
            typeof(Transaction).GetProperty(nameof(Transaction.TransactionId))!
                .SetValue(expected, transactionId);

            var repoMock = new Mock<ITransactionRepository>();
            repoMock.Setup(r => r.GetByIdAsync(transactionId)).ReturnsAsync(expected);

            var service = new TransactionService(repoMock.Object, _loggerMock.Object);

            var result = await service.GetTransactionByIdAsync(transactionId);

            Assert.NotNull(result);
            Assert.Equal(transactionId, result!.TransactionId);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_Should_Return_Null_When_Not_Found()
        {
            var transactionId = Guid.NewGuid();
            var repoMock = new Mock<ITransactionRepository>();
            repoMock.Setup(r => r.GetByIdAsync(transactionId)).ReturnsAsync((Transaction?)null);

            var service = new TransactionService(repoMock.Object, _loggerMock.Object);

            var result = await service.GetTransactionByIdAsync(transactionId);

            Assert.Null(result);
            repoMock.Verify(r => r.GetByIdAsync(transactionId), Times.Once);
        }

        [Fact]
        public async Task GetAllTransactionsAsync_Should_Return_Transactions_From_Repository()
        {
            var accountId = Guid.NewGuid();
            var transactions = new List<Transaction>
            {
                new Transaction(accountId, 5),
                new Transaction(accountId, -2),
            };

            var repoMock = new Mock<ITransactionRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(transactions);

            var service = new TransactionService(repoMock.Object, _loggerMock.Object);

            var result = await service.GetAllTransactionsAsync();

            Assert.NotNull(result);
            Assert.Equal(transactions.Count, result.Count);
            Assert.Equal(transactions.Select(t => t.TransactionId), result.Select(t => t.TransactionId));
            repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateTransactionAsync_Should_Pass_Correct_Values_To_Repository()
        {
            var captured = (Transaction?)null;
            var repoMock = new Mock<ITransactionRepository>();
            repoMock
                .Setup(r => r.AddAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(t => captured = t)
                .ReturnsAsync((Transaction t) => t);

            var service = new TransactionService(repoMock.Object, _loggerMock.Object);
            var accountId = Guid.NewGuid();
            var amount = 42;

            var result = await service.CreateTransactionAsync(accountId, amount);

            Assert.NotNull(captured);
            Assert.Equal(accountId, captured!.AccountId);
            Assert.Equal(amount, captured.Amount);
            Assert.Equal(result.TransactionId, captured.TransactionId);
            repoMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public async Task CreateTransactionAsync_Should_Propagate_TransactionLimitExceededException()
        {
            var repoMock = new Mock<ITransactionRepository>();
            repoMock
                .Setup(r => r.AddAsync(It.IsAny<Transaction>()))
                .ThrowsAsync(new TransactionLimitExceededException(10));

            var service = new TransactionService(repoMock.Object, _loggerMock.Object);

            await Assert.ThrowsAsync<TransactionLimitExceededException>(
                () => service.CreateTransactionAsync(Guid.NewGuid(), 1));
        }

        [Fact]
        public async Task CreateTransactionAsync_Should_Propagate_TransactionAlreadyExistException()
        {
            var duplicateId = Guid.NewGuid();
            var repoMock = new Mock<ITransactionRepository>();
            repoMock
                .Setup(r => r.AddAsync(It.IsAny<Transaction>()))
                .ThrowsAsync(new TransactionAlreadyExistException(duplicateId));

            var service = new TransactionService(repoMock.Object, _loggerMock.Object);

            await Assert.ThrowsAsync<TransactionAlreadyExistException>(
                () => service.CreateTransactionAsync(Guid.NewGuid(), 1));
        }
    }
}

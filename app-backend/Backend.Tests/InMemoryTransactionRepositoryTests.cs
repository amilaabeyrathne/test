using Backend.Domain.Entities;
using Backend.Domain.Exceptions;
using Backend.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Backend.Tests;

public class InMemoryTransactionRepositoryTests
{
    [Fact]
    public async Task AddAsync_Should_Throw_When_Transaction_Limit_Exceeded()
    {
        var limit = 25;
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["MaxTransactionLimit"] = limit.ToString()
            })
            .Build();

        var repository = new InMemoryTransactionRepository(configuration);
        var accountId = Guid.NewGuid();

        for (var i = 0; i < limit; i++)
        {
            var transaction = new Transaction(accountId, 1);
            await repository.AddAsync(transaction);
        }

        var extraTransaction = new Transaction(accountId, 1);

        var exception = await Assert.ThrowsAsync<TransactionLimitExceededException>(
            () => repository.AddAsync(extraTransaction));

        Assert.Equal($"Transaction limit of {limit} has been reached.", exception.Message);
    }
}


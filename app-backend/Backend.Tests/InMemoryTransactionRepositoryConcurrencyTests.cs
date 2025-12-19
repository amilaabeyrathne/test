using Backend.Domain.Entities;
using Backend.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Backend.Tests
{
    public class InMemoryTransactionRepositoryConcurrencyTests
    {

        [Fact]
        public async Task AddAsync_Should_Handle_High_Concurrency_With_Unique_Transactions()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["MaxTransactionLimit"] = "25"
                })
                .Build();

            var repo = new InMemoryTransactionRepository(configuration);
            var accountId = Guid.NewGuid();
            var total = 25;

            var tasks = new List<Task>();
            for (var item = 0; item < total; item++)
            {
                var t = new Transaction(accountId, item % 2 == 0 ? 1 : -1);
                tasks.Add(repo.AddAsync(t));
            }

            await Task.WhenAll(tasks);

            var count = await repo.GetTotalCountAsync();
            var all = await repo.GetAllAsync();

            Assert.Equal(total, count);
            Assert.Equal(total, all.Count);
            Assert.Equal(total, all.Select(t => t.TransactionId).Distinct().Count());
        }
    }
}

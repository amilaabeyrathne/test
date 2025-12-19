using System.Net;
using System.Net.Http.Json;
using Backend.WebApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Backend.Tests.Api
{
    public class AccountsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AccountsApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(_ => { });
        }

        [Fact]
        public async Task Get_Account_Should_Return_NotFound_When_No_Transactions()
        {
            var client = _factory.CreateClient();

            var accountId = Guid.NewGuid();
            var response = await client.GetAsync($"/accounts/{accountId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Account_Should_Return_Balance_After_Transactions()
        {
            var client = _factory.CreateClient();
            var accountId = Guid.NewGuid();

            // create two transactions for account
            await client.PostAsJsonAsync("/transactions", new TransactionRequestModel { AccountId = accountId, Amount = 100 });
            await client.PostAsJsonAsync("/transactions", new TransactionRequestModel { AccountId = accountId, Amount = -30 });

            var response = await client.GetAsync($"/accounts/{accountId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var account = await response.Content.ReadFromJsonAsync<AccountResponseModel>();
            Assert.NotNull(account);
            Assert.Equal(accountId, account!.AccountId);
            Assert.Equal(70, account.Balance);
        }
    }
}


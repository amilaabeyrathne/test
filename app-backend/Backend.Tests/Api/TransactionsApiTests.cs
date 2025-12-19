using System.Net;
using System.Net.Http.Json;
using Backend.WebApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Backend.Tests.Api
{
    public class TransactionsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TransactionsApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(_ => { });
        }

        [Fact]
        public async Task Post_Transactions_Should_Create_And_Return_CreatedAt()
        {
            var client = _factory.CreateClient();
            var request = new TransactionRequestModel
            {
                AccountId = Guid.NewGuid(),
                Amount = 10
            };

            var response = await client.PostAsJsonAsync("/transactions", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var body = await response.Content.ReadFromJsonAsync<TransactionResponseModel>();
            Assert.NotNull(body);
            Assert.Equal(request.AccountId, body!.AccountId);
            Assert.Equal(request.Amount, body.Amount);
            Assert.NotEqual(Guid.Empty, body.TransactionId);
        }

        [Fact]
        public async Task Get_Transactions_Id_Should_Return_404_When_Not_Found()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/transactions/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Transactions_Should_Return_List()
        {
            var client = _factory.CreateClient();

            // Create one transaction
            var request = new TransactionRequestModel
            {
                AccountId = Guid.NewGuid(),
                Amount = 5
            };

            var created = await client.PostAsJsonAsync("/transactions", request);
            Assert.Equal(HttpStatusCode.Created, created.StatusCode);

            var listResponse = await client.GetAsync("/transactions");
            Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

            var list = await listResponse.Content.ReadFromJsonAsync<List<TransactionResponseModel>>();
            Assert.NotNull(list);
            Assert.True(list!.Count >= 1);
        }
    }
}


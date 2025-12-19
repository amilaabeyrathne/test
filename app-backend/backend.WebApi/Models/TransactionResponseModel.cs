using System.Text.Json.Serialization;

namespace Backend.WebApi.Models
{
    public class TransactionResponseModel
    {
        [JsonPropertyName("transaction_id")]
        public Guid TransactionId { get; set; }

        [JsonPropertyName("account_id")]
        public Guid AccountId { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        public int Amount { get; set; }
    }
}

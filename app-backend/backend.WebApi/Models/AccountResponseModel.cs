using System.Text.Json.Serialization;

namespace Backend.WebApi.Models
{
    public class AccountResponseModel
    {
        [JsonPropertyName("account_id")]
        public Guid AccountId { get; set; }
        public int Balance { get; set; }
    }
}

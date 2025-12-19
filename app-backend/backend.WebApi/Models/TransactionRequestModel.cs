using Backend.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.WebApi.Models
{
    public class TransactionRequestModel
    {
        [Required(ErrorMessage = "AccountId is required.")]
        [NotEmptyGuid(ErrorMessage = "Account ID cannot be empty.")]
        [JsonPropertyName("account_id")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [ValidAmount(ErrorMessage = "Amount is not in the valid range. ")]
        public int Amount { get; set; }
    }
}

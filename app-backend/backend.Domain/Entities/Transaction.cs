namespace Backend.Domain.Entities
{
    public class Transaction
    {
        public Guid TransactionId { get;  private set; }
        public Guid AccountId { get; private set; }
        public int Amount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Transaction(Guid accountId, int amount)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Account ID cannot be empty");

            if (amount == 0)
                throw new ArgumentException("Amount cannot be zero");

            TransactionId = Guid.NewGuid();
            AccountId = accountId;
            Amount = amount;
            CreatedAt = DateTime.UtcNow;
        }
    }
}

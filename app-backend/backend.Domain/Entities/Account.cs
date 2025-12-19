namespace Backend.Domain.Entities
{
    public class Account
    {
        public Guid AccountId { get; private set; }
        public int Balance { get; private set; }

        public Account(Guid accountId, int balance)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Account ID cannot be empty");

            AccountId = accountId;
            Balance = balance;
        }
    }
}

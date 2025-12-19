namespace Backend.Domain.Exceptions
{
    public class TransactionLimitExceededException : Exception
    {
        public TransactionLimitExceededException(int limit)
            : base($"Transaction limit of {limit} has been reached.")
        {
        }
    }
}

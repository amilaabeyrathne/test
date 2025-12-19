namespace Backend.Domain.Exceptions
{
    public class TransactionAlreadyExistException: Exception
    {
        public TransactionAlreadyExistException(Guid transactionId)
            : base($"Transaction with ID '{transactionId}' already exists.")
        {
        }
    }
}
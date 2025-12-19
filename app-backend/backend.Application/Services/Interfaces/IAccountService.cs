using Backend.Domain.Entities;

namespace Backend.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByIdAsync(Guid accountId);
    }
}

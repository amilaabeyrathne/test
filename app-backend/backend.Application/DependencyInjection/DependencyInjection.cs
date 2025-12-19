using Backend.Application.Services;
using Backend.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.DependancyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}

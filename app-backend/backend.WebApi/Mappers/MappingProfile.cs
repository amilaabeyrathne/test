using AutoMapper;
using Backend.Domain.Entities;
using Backend.WebApi.Models;

namespace Backend.WebApi.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Transaction mappings
            CreateMap<Transaction, TransactionResponseModel>();

            // Account mappings
            CreateMap<Account, AccountResponseModel>();
        }
    }
}

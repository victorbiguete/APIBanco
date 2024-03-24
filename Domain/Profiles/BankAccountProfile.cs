using AutoMapper;

using APIBanco.Domain.Dtos;
using APIBanco.Domain.Models.DbContext;
namespace APIBanco.Domain.Profiles;

public class BankAccountProfile : Profile
{
    public BankAccountProfile()
    {
        CreateMap<BankAccount, BankAccountResponseDto>();
    }
}

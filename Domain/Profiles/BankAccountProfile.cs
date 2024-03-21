using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;
namespace APIBanco.Domain.Profiles;

public class BankAccountProfile : Profile
{
    public BankAccountProfile()
    {
        CreateMap<BankAccount, BankAccountResponseDto>();
    }
}

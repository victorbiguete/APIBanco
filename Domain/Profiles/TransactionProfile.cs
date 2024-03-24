using AutoMapper;

using APIBanco.Domain.Dtos;
using APIBanco.Domain.Models.DbContext;

namespace APIBanco.Domain.Profiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transactions, TransactionResponseDto>();
        CreateMap<TransactionRequestDto, Transactions>();
    }
}

using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Profiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transactions, TransactionResponseDto>();
        // CreateMap<List<Transactions>, List<TransactionResponseDto>>();
        CreateMap<TransactionRequestDto, Transactions>();
    }
}

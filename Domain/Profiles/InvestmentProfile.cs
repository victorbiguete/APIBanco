using AutoMapper;


using APIBanco.Domain.Dtos;
using APIBanco.Domain.Models.DbContext;

namespace APIBanco.Domain.Profiles;

public class InvestmentProfile : Profile
{
    public InvestmentProfile()
    {
        CreateMap<Investment, InvestmentResponseDto>();
        CreateMap<InvestmentRequestDto, Investment>();
    }
}

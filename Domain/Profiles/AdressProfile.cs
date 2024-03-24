using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Profiles;

public class AdressProfile : Profile
{
    public AdressProfile()
    {
        CreateMap<Adress, AdressResponseDto>().ReverseMap();
        CreateMap<AdressRequestDto, Adress>().ReverseMap();
    }
}
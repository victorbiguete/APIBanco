using AutoMapper;

using APIBanco.Domain.Dtos;
using APIBanco.Domain.Models.DbContext;

namespace APIBanco.Domain.Profiles;

public class AdressProfile : Profile
{
    public AdressProfile()
    {
        CreateMap<Adress, AdressResponseDto>().AfterMap((src, dest) =>
        {
            dest.State = src.UF;
            dest.number = src.HouseNumber;
        });
        CreateMap<AdressRequestDto, Adress>().AfterMap((src, dest) =>
        {
            dest.UF = src.State;
            dest.HouseNumber = src.number;
        });
    }
}

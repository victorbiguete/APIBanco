using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Profiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientResponseDto>().AfterMap((src, dest) => dest.Adress = "/api/adress/cpf/" + src.Cpf);
        CreateMap<ClientRequestDto, Client>().ReverseMap();
        CreateMap<Client, ClientRequestNoCpfDto>().ReverseMap();
    }

}
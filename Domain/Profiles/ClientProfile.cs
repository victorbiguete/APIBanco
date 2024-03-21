using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Profiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientResponseDto>();
        CreateMap<ClientRequestDto, Client>().AfterMap((src, dest) =>
        {
            dest.Adress.Cpf = src.Cpf;
        });
        CreateMap<Client, ClientRequestNoCpfDto>();
    }

}
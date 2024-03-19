using AutoMapper;

using APIBanco.Domain.Models;
using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Profiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientResponseDto>().ReverseMap();
        CreateMap<ClientRequestDto, Client>().ReverseMap();
    }
}

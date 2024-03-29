using AutoMapper;

using APIBanco.Domain.Dtos;
using APIBanco.Domain.Models.DbContext;

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
        CreateMap<Client, ClientRequestUpdateDto>();
    }

}
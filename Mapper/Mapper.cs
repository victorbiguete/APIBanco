using APIBanco.Model;
using APIBanco.DTOs;
using AutoMapper;

namespace APIBanco.Mapper
{
    public class Mapper  : Profile
    {
        public Mapper() 
        {
            //T -> Cliente, U -> ClienteDTO
            //Criei o DTO Generico pra facilitar o mapeamento, vamos ver se funciona
            CreateMap<ContaCorrente, ContaCorrenteDTO>();
            CreateMap<ContaCorrenteDTO, ContaCorrente>();

            CreateMap<Cliente, ClienteDTO>();
            CreateMap<ClienteDTO, Cliente>();

            CreateMap<ContaPoupanca, ContaPoupancaDTO>();
            CreateMap<ContaPoupancaDTO, ContaPoupancaDTO>();
        }
    }
}

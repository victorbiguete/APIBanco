using AutoMapper;

namespace APIBanco.Mapper
{
    public class Mapper <T, U> : Profile
    {
        public Mapper() 
        {
            //T -> Cliente, U -> ClienteDTO
            //Criei o DTO Generico pra facilitar o mapeamento, vamos ver se funciona
            CreateMap<T, U>();
            CreateMap<U, T>();
        }
    }
}

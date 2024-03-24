using APIBanco.Model;

namespace APIBanco.Model
{
    public class Tarifa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Valor { get; set; }
        public List<Servicos> Servicos { get; set; }

        //TODO CRUD e ToString()
    }
}

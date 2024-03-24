using APIBanco.Services;
using Microsoft.AspNetCore.Identity;
namespace APIBanco.Model
{
    public class Cliente:IdentityUser
    {
        public string Nome { get; private set; }
        public int AnoNascimento { get; private set; }
        public string Cpf { get; private set; }
        public List<Conta> Contas { get; set; }
        public string? Cnpj { get; private set; }
        public decimal Renda { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public DateTime DataObito { get; private set; }

        public Cliente(string nome, string cpf, int anoNascimento):base()
        {
            if (int.Parse(DateTime.Now.ToString("yyyy")) - anoNascimento < 18)
                throw new ArgumentException("O cliente deve ter mais de 18 anos");
            
            if (cpf.Length != 11)
                throw new ArgumentException("O CPF deve possuir 11 digitos !");
            
            if (!Validation.ValidarCPF(cpf))
                throw new ArgumentException("O CPF deve ser valido !");



            Nome = nome;
            AnoNascimento = anoNascimento;
            Cpf = cpf;

        }
    }
}


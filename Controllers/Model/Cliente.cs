namespace APIBanco.Controllers.Model
{
    public class Cliente
    {
        public string Nome { get; private set; }

        public int AnoNascimento { get; private set; }
        public string Cpf { get; private set; }
        public List<Conta> Contas { get; set; }

        public Cliente(string nome, string cpf, int anoNascimento)
        {
            if (Int32.Parse(DateTime.Now.ToString("yyyy")) - anoNascimento < 18)
                throw new System.ArgumentException("O cliente deve ter mais de 18 anos");
            if (cpf.Length != 11)
                throw new System.ArgumentException("O CPF deve possuir 11 digitos!");

            Nome = nome;
            AnoNascimento = anoNascimento;
            Cpf = cpf;
        }
    }
}


using System.ComponentModel;
namespace APIBanco.Services
{
    public abstract class Servicos
    {
        public static string VerificarVazio()
        {
            string entrada = null;
            while (string.IsNullOrWhiteSpace(entrada))
            {
                entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(entrada))
                    throw new ArgumentException("Entrada Invalida. Digite Novamente:");
            }
            return entrada;
        }

        public static bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;
            // Remover caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verificar se o CPF possui 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verificar se todos os dígitos são iguais (caso especial)
            if (cpf.Distinct().Count() == 1)
                return false;

            // Calcular os dígitos verificadores
            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];
            }

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf = tempCpf + digito1;
            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];
            }

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            // Verificar se os dígitos calculados correspondem aos dígitos informados
            return cpf.EndsWith(digito1.ToString() + digito2.ToString());
        }

        
    }
    
}

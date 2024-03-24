using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Models.DbContext;


public class Client
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Key]
    [Required]
    public string Cpf { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime BornDate { get; set; }
    public virtual Adress Adress { get; set; } = new Adress();
    public virtual BankAccount BankAccount { get; set; } = new BankAccount();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void ValidateCpf()
    {
        if (string.IsNullOrWhiteSpace(Cpf))
            throw new Exception(message: "Invalid Cpf.");

        // Remover caracteres não numéricos
        Cpf = new string(Cpf.Where(char.IsDigit).ToArray());

        // Verificar se o CPF possui 11 dígitos
        if (Cpf.Length != 11)
            throw new Exception(message: "Invalid Cpf.");

        // Verificar se todos os dígitos são iguais (caso especial)
        if (Cpf.Distinct().Count() == 1)
            throw new Exception(message: "Invalid Cpf.");

        // Calcular os dígitos verificadores
        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = Cpf.Substring(0, 9);
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
        var isValid = Cpf.EndsWith(digito1.ToString() + digito2.ToString());

        if (isValid == false)
        {
            throw new Exception(message: "Invalid Cpf.");
        }
    }
}


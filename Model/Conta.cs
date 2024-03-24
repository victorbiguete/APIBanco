using Microsoft.Extensions.Options;
using APIBanco.Enum;

namespace APIBanco.Model
{
    public class Conta
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int Agencia { get; set; } = 1;
        public decimal Saldo { get; private set; } = 0;
        public Cliente Titular { get; set; }
        public StatusServico Status { get; set; }

        public Conta()
        {
        }

        public Conta(string numero, int agencia, decimal saldo, Cliente titular)
        {
            Saldo = saldo;
            Numero = numero;
            Agencia = agencia;
            Titular = titular;
        }
        
        public bool Sacar(decimal valor)
        {
            if (Saldo < valor)
            {
                throw new ArgumentOutOfRangeException("Valor do Saque", valor, SaqueMaiorQueSaldoMessage);
            }
            if (valor < 0)
            {
                throw new ArgumentOutOfRangeException("Valor do Saque", valor, SaqueMenorQueZeroMessage);
            }

            Saldo -= valor;
            return true;
        }
        public bool Depositar(decimal valor)
        {
            if (valor < 0)
                throw new ArgumentOutOfRangeException("Deposito Invalido", valor.ToString(), "Valor para deposito não pode ser " +
                    "negativo");
            
            Saldo += valor;
            return true;
        }
        public bool Trasferir(Conta destino, decimal valor)
        {
            if (Sacar(valor))
            {
                destino.Depositar(valor);
                return true;
            }
            return false;
        }
    }
}


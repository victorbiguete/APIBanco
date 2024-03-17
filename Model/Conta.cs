using Microsoft.Extensions.Options;
using APIBanco.Enum;

namespace APIBanco.Model
{
    public class Conta
    {
        public const string SaqueMenorQueZeroMessage = "Valor informado para saque não pode ser negativo";
        public const string SaqueMaiorQueSaldoMessage = "Valor informado para sqque ultrapassa o saldo disponível";
        private string _numero { get; set; }
        private int _agencia { get; set; } = 1;
        protected decimal _saldo { get; set; } = 0;
        private decimal _saldoTotal { get; set; }
        private string _contaMaiorSaldo { get; set; } = "";
        private decimal _maiorSaldo { get; set; } = 0;
        public Cliente Titular { get; set; }
        public StatusServico Status { get; set; }
        public Conta(string numero, Cliente titular)
        {
            _numero = numero;
            Titular = titular;
        }

        public Conta(string numero, int agencia, decimal saldo, Cliente titular)
        {
            _saldo = saldo;
            _numero = numero;
            _agencia = agencia;
            _saldoTotal += saldo;
            Titular = titular;
            if (_saldo > _maiorSaldo)
            {
                _maiorSaldo = _saldo;
                _contaMaiorSaldo = _numero;
            }

        }
        public string Numero
        {
            get => _numero;
            private set => _numero = value;
        }
        public decimal Saldo
        {
            get => _saldo;
            private set => _saldoTotal = value;
        }

        public decimal SaldoTotal
        {
            get => _saldoTotal;
            private set => _saldoTotal = value;
        }

        public string ContaMaiorSaldo
        {
            get => _contaMaiorSaldo;
        }



        public bool Sacar(decimal valor)
        {
            if (_saldo < valor)
            {
                throw new ArgumentOutOfRangeException("Valor do Saque", valor, SaqueMaiorQueSaldoMessage);
            }
            if (valor < 0)
            {
                throw new ArgumentOutOfRangeException("Valor do Saque", valor, SaqueMenorQueZeroMessage);
            }

            _saldo -= valor;
            return true;
        }
        public bool Depositar(decimal valor)
        {
            if (valor < 0)
            {
                throw new ArgumentOutOfRangeException("Deposito Invalido", valor.ToString(), "Valor para deposito não pode ser " +
                    "negativo");
            }

            _saldo += valor;
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


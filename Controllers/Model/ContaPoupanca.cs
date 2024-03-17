using System;


namespace APIBanco.Controllers.Model
{
    public class ContaPoupanca
    {
        private Conta conta;
        private decimal saldoPoupanca;
        private int aniversario;

        public ContaPoupanca(Conta conta, decimal saldoPoupanca, int aniversario)
        {
            this.conta = conta;
            this.saldoPoupanca = saldoPoupanca;
            this.aniversario = aniversario;
        }

        public Conta GetConta()
        {
            return conta;
        }

        public void SetConta(Conta conta)
        {
            this.conta = conta;
        }

        public decimal GetSaldoPoupanca()
        {
            return saldoPoupanca;
        }

        public void SetSaldoPoupanca(decimal saldoPoupanca)
        {
            this.saldoPoupanca = saldoPoupanca;
        }

        public int GetAniversario()
        {
            return aniversario;
        }

        public void SetAniversario(int aniversario)
        {
            this.aniversario = aniversario;
        }

        public void Depositar(decimal valor)
        {
            saldoPoupanca += valor;
        }

        public bool Sacar(decimal valor)
        {
            if (saldoPoupanca >= valor)
            {
                saldoPoupanca -= valor;
                return true;
            }
            else
            {
                Console.WriteLine("Saldo insuficiente na poupança.");
                return false;
            }
        }

    }
}

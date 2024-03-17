namespace APIBanco.Model
{
    public class ContaCorrente : Conta
    {
        public ContaCorrente(string numero, Cliente titular) : base(numero, titular)
        {
        }

        public ContaCorrente(string numero, int agencia, decimal saldo, Cliente titular)
            : base(numero, agencia, saldo, titular)
        {
        }
        //Apagar esse, vamos colocar o limite como uma porcentagem da renda
        private decimal _limite = 1000;
        public decimal Limite
        {
            get { return _limite; }
            private set { _limite = value; }
        }
    }
}

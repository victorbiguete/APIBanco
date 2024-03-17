namespace APIBanco.Controllers.Model
{
    public class ContaEspecial : Conta
    {
        public ContaEspecial(string numero, Cliente titular) : base(numero, titular)
        {
        }

        public ContaEspecial(string numero, int agencia, decimal saldo, Cliente titular) 
            : base(numero, agencia, saldo, titular)
        {
        }
        private decimal _limite = 1000;
        public decimal Limite
        {
            get { return _limite; }
            private set { _limite = value; }
        }
    }
}

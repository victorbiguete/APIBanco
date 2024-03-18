namespace APIBanco.Security
{
    public abstract class Key
    {
        public static string Secret = GeradorChaveSecreta.Gerador(64);
    }
}

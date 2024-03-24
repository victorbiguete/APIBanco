using System.Security.Cryptography;

namespace APIBanco.Security
{
    public abstract class GeradorChaveSecreta
    {
        public static string Gerador(int length)
        {
            // Define os caracteres permitidos na chave secreta
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

            // Inicializa o gerador de números aleatórios criptograficamente seguro
            using (var rng = new RNGCryptoServiceProvider())
            {
                var data = new byte[length];
                rng.GetBytes(data);

                // Converte os bytes aleatórios em uma string usando os caracteres permitidos
                var result = new char[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = chars[data[i] % chars.Length];
                }

                return new string(result);
            }
        }
    }
}

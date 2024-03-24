
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIBanco.Security
{
    public class TokenService
    {
        //public static object GenerateToken(Conta conta)
        //{
        //    //Key Secret pode ser uma string de conteudo aleatorio
        //    var key = Encoding.ASCII.GetBytes(Key.Secret);

        //    //Configuração do Token contendo:
        //    //Subject - A que aquele token vai estar atrelado
        //    //Expires - Tempo de vida dele
        //    //SigningCredentials
        //    var tokenConfig = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim("contaId",conta.Id.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(3),
        //        SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenConfig);
        //    var tokenString = tokenHandler.WriteToken(token);

        //    return new
        //    {
        //        token = tokenString
        //    };
        //}
    }
}

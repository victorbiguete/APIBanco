using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIBanco.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace APIBanco.Services;

public class JwtService
{
    private JwtConfig _jwtConfig;

    public JwtService() { }

    public JwtService(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }
    public string GenerateToken(Client client)
    {
        JwtSecurityTokenHandler? jwtTokenHandler = new JwtSecurityTokenHandler();
        byte[]? key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        SecurityTokenDescriptor? tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]{
                new Claim("Id", client.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, client.BornDate.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, client.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }),

            Expires = DateTime.UtcNow.AddHours(1),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken? token = jwtTokenHandler.CreateToken(tokenDescriptor);

        string? jwtToken = jwtTokenHandler.WriteToken(token);

        return jwtToken;
    }

}

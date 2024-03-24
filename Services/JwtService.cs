using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIBanco.Domain.Models.DbContext;
using APIBanco.Domain.Models.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace APIBanco.Services;

public class JwtService
{
    private IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(Client client)
    {
        JwtSecurityTokenHandler? jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]!);

        SecurityTokenDescriptor? tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]{
                new Claim("Id", client.Id.ToString()),
                new Claim("Cpf", client.Cpf),
                new Claim(JwtRegisteredClaimNames.Sub, client.BornDate.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, client.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }),

            Expires = DateTime.UtcNow.AddHours(1),
            // Expires = DateTime.UtcNow.AddDays(1),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken? token = jwtTokenHandler.CreateToken(tokenDescriptor);

        string? jwtToken = jwtTokenHandler.WriteToken(token);

        return jwtToken;
    }

    public int GetIdClaimToken(ClaimsPrincipal User)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            int userIdValue = int.Parse(userIdClaim!.Value);

            return userIdValue;
        }
        catch (Exception)
        {
            throw new TokenIdNotEqualsClientIdException("Claim not found in token");
        }
    }

    public string GetCpfClaimToken(ClaimsPrincipal User)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Cpf");

            string userCpfValue = userIdClaim!.Value;

            return userCpfValue;
        }
        catch (Exception)
        {
            throw new TokenIdNotEqualsClientIdException("Claim not found in token");
        }
    }
}

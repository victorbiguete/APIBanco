using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIBanco.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace APIBanco.Services;

public class JwtService
{
    private IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// Generates a JWT token for the provided client.
    /// </summary>
    /// <param name="client">The client for whom the token is being generated.</param>
    /// <returns>The generated JWT token.</returns>
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

            // Expires = DateTime.UtcNow.AddMinutes(1),
            Expires = DateTime.UtcNow.AddHours(1),
            // Expires = DateTime.UtcNow.AddDays(1),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken? token = jwtTokenHandler.CreateToken(tokenDescriptor);

        string? jwtToken = jwtTokenHandler.WriteToken(token);

        return jwtToken;
    }

    /// <summary>
    /// Retrieves the Id claim from the JWT token.
    /// </summary>
    /// <param name="User">The principal representing the user.</param>
    /// <returns>The Id value from the claim.</returns>
    /// <exception cref="TokenIdNotEqualsClientIdException">Thrown when the Id claim is not found in the token.</exception>
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

    /// <summary>
    /// Retrieves the Cpf claim from the JWT token.
    /// </summary>
    /// <param name="User">The principal representing the user.</param>
    /// <returns>The Cpf value from the claim.</returns>
    /// <exception cref="TokenIdNotEqualsClientIdException">Thrown when the Cpf claim is not found in the token.</exception>
    /// <remarks>
    /// This method retrieves the Cpf claim from the token.
    /// If the claim is not found, it throws a <see cref="TokenIdNotEqualsClientIdException"/> exception.
    /// </remarks>
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

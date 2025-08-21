using CryptoMonitoring.Common.Interfaces.Auth;
using CryptoMonitoring.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CryptoMonitoring.Common.Services.Auth;

public class JwtService : IJwtService
{
    private readonly string _jwtSecretKey;
    private readonly string _jwtExpiresDays;

    public JwtService(IConfiguration configuration)
    {
        _jwtSecretKey = configuration["JWT_SECRET_KEY"]!;
        _jwtExpiresDays = configuration["JWT_EXPIRES_DAYS"]!;
    }

    public string GenerateToken(User user)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey)),
            SecurityAlgorithms.HmacSha256);

        Claim[] claims = [
            new("user_id", user.Id.ToString()),
            new("role", user.Role)
        ];

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddDays(int.Parse(_jwtExpiresDays))
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    public Guid GetUserIdFromToken(string authHeader)
    {
        var token = authHeader["Bearer ".Length..].Trim();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        return Guid.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == "user_id")!.Value);
    }
}
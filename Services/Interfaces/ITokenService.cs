using back.Models;
using System.IdentityModel.Tokens.Jwt;

namespace back.Services.Interfaces;

public interface ITokenService
{
    Task<JwtSecurityToken> GenerateTokenAsync(User user);
    string GetUserNameFromToken(string token);
}

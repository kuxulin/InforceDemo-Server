using back.Models;
using System.IdentityModel.Tokens.Jwt;

namespace back.Interfaces;

public interface ITokenService
{
    Task<JwtSecurityToken> GenerateTokenAsync(User user);
}

using back.Interfaces;
using back.Models;
using back.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace back.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    public AuthController(UserManager<User> userManager,ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LogIn(LoginViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Name);
        if(user is null)
        {
            return Unauthorized("Cannot find user in system");
        }

        if(await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var token = await _tokenService.GenerateTokenAsync(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized("Passwords arent equal");
    }
}

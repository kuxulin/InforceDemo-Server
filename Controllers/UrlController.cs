using back.Models;
using back.Models.Consts;
using back.Models.ViewModels;
using back.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace back.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UrlController(IUrlService urlService, IUserService userService, ITokenService tokenService)
    {
        _urlService = urlService;
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<IEnumerable<Url>> GetUrlsAsync()
    {

        return await _urlService.GetUrlsAsync();
    }

    [HttpPost]
    [Authorize(Policy = UserPolicy.PARTNER_ADMIN)]
    public async Task<IActionResult> CreateUrl([FromBody] CreateUrlViewModel model)
    {
        if (!Uri.TryCreate(model.LongVersion, UriKind.Absolute, out _))
        {
            return BadRequest(
            new
            {
                text = "Invalid url"
            });
        }

        var user = await GetUserFromToken(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
        var shortUrlStart = $"{Request.Scheme}://{Request.Host}/";

        var url = new Url()
        {
            LongVersion = model.LongVersion,
            ShortVersion = shortUrlStart,
            User = user
        };

        try
        {
            var shortUrl = await _urlService.CreateUrlAsync(url);

            return Ok(new
            {
                shortUrl
            });
        }
        catch (ApplicationException ex)
        {
            return BadRequest(
            new
            {
                text = ex.Message
            });
        }
    }

    private async Task<User> GetUserFromToken(string token)
    {
        var userName = _tokenService.GetUserNameFromToken(token);
        return await _userService.GetUserByNameAsync(userName);
    }

    [HttpDelete]
    [Authorize(Policy = UserPolicy.PARTNER_ADMIN)]
    public async Task<IActionResult> DeleteUrl(Guid id)
    {
        var user = await GetUserFromToken(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
        var url = await _urlService.GetUrlByIdAsync(id);

        if (url is null)
        {
            return BadRequest(new { text = "There is no such url in db" });
        }

        var userRoles = await _userService.GetUserRolesAsync(user);

        if(url.User == user || userRoles.Contains("Admin"))
        {
            await _urlService.DeleteUrlAsync(url);
            return Ok();
        }

        return BadRequest(new { text = "Access denied" });
    }
}

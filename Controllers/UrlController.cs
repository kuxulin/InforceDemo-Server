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

        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userName = _tokenService.GetUserNameFromToken(token);
        var user = await _userService.GetUserByNameAsync(userName);
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
}

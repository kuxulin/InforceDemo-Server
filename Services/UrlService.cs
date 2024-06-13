using back.Models;
using back.Repositories.Interfaces;
using back.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;

namespace back.Services;

public class UrlService : IUrlService
{
    private readonly IUrlRepository _urlRepository;

    public UrlService(IUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
    }

    public async Task<IEnumerable<Url>> GetUrlsAsync()
    {
        return await _urlRepository.GetUrls().ToListAsync();
    }

    public async Task<string> CreateUrlAsync(Url url)
    {
        var possibleUrl = await GetUrlByLongVersion(url.LongVersion);

        if (possibleUrl != null)
        {
            throw new ApplicationException("Url is not unique!");
        }

        url.Code = await MakeShortVersion(url.LongVersion);
        url.ShortVersion += url.Code;       
        await _urlRepository.AddUrlAsync(url);

        return url.ShortVersion;
    }

    public async Task<Url> GetUrlByLongVersion(string longVersion)
    {
        return await _urlRepository.GetUrls().Where(u => u.LongVersion == longVersion).FirstOrDefaultAsync();
    }

    private async Task<string> MakeShortVersion(string longVersion)
    {
        int length = 7;
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var codeChars = new char[length];
        int maxValue = alphabet.Length;
        var random = new Random();
        while (true)
        {
            for (var i = 0; i < length; i++)
            {
                var randomIndex = random.Next(maxValue);

                codeChars[i] = alphabet[randomIndex];
            }

            var code = new string(codeChars);

            if (!await _urlRepository.CodeExists(code))
            {
                return code;
            }
        }
    }
}

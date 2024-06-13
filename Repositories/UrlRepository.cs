using back.Contexts;
using back.Models;
using back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back.Repositories;

public class UrlRepository : IUrlRepository
{
    private readonly DatabaseContext _context;

    public UrlRepository(DatabaseContext context)
    {
        _context = context;
    }

    public IQueryable<Url> GetUrls()
    {
        return _context.Urls;
    }

    public async Task AddUrlAsync(Url url)
    {
        await _context.Urls.AddAsync(url);
        await _context.SaveChangesAsync();
    }


    public async Task<bool> CodeExists(string code)
    {
        return await _context.Urls.AnyAsync(x => x.Code == code);
    }
}


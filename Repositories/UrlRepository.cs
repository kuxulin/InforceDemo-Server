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
        _context.Urls.Add(url);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CodeExists(string code)
    {
        return await _context.Urls.AnyAsync(x => x.Code == code);
    }

    public async Task DeleteUrlAsync(Url url)
    {
        _context.Urls.Remove(url);
        await _context.SaveChangesAsync();
    }
}


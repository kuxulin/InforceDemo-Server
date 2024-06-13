using back.Models;

namespace back.Repositories.Interfaces;

public interface IUrlRepository
{
    IQueryable<Url> GetUrls();
    Task AddUrlAsync(Url url);
    Task<bool> CodeExists(string code);
    Task DeleteUrlAsync(Url url);
}

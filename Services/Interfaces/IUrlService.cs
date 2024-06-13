using back.Models;

namespace back.Services.Interfaces;

public interface IUrlService
{
    Task<IEnumerable<Url>> GetUrlsAsync();
    Task<string> CreateUrlAsync(Url url);
    Task<Url> GetUrlByLongVersion(string longVersion);
    Task<Url> GetUrlByIdAsync(Guid id);
    Task DeleteUrlAsync(Url url);
}

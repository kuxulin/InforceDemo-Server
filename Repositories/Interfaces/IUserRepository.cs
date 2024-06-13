using back.Models;

namespace back.Repositories.Interfaces;

public interface IUserRepository
{
    IQueryable<User> GetUsers();
    Task<IEnumerable<string>> GetUserRolesAsync(User user);
}

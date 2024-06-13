using back.Contexts;
using back.Models;
using back.Repositories.Interfaces;

namespace back.Repositories;

public class UserRepository :IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public IQueryable<User> GetUsers()
    {
        return _context.Users;
    }
}

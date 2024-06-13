using back.Contexts;
using back.Models;
using back.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Net.WebSockets;

namespace back.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;
    private readonly UserManager<User> _userManager;

    public UserRepository(DatabaseContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IQueryable<User> GetUsers()
    {
        return _context.Users;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}

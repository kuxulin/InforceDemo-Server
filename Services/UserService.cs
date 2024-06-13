using back.Models;
using back.Repositories.Interfaces;
using back.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back.Services;

public class UserService :IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetUserByNameAsync(string userName)
    {
        return await _userRepository.GetUsers().Where(u => u.UserName == userName).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(User user)
    {
        return await _userRepository.GetUserRolesAsync(user);
    }
}

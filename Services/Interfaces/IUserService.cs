﻿using back.Models;

namespace back.Services.Interfaces;

public interface IUserService
{
    Task<User> GetUserByNameAsync(string userName);
    Task<IEnumerable<string>> GetUserRolesAsync(User user);
}

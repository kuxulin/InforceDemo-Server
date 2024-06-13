using Microsoft.AspNetCore.Identity;
using back.Contexts;
using back.Models;
using Microsoft.EntityFrameworkCore;

namespace back.Extensions;

public static class DataSeeder
{
    public static async Task SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<DatabaseContext>();
        using var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
        using var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();

        if (await context.Users.AnyAsync())
            return;

        Role[] roles =
        {
            new() {Name = "Partner"},
            new() {Name = "Admin"},
        };

        User[] users =
        {
            new() { UserName="admin1"},
            new() { UserName="user1"},
            new() { UserName="user2"}
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        for (int i = 0; i < users.Length; i++)
        {
            await userManager.CreateAsync(users[i], "string");
        }


        await userManager.AddToRolesAsync(users[0], roles.Select(r => r.Name)!);
        await userManager.AddToRolesAsync(users[1], roles.Select(r => r.Name)!.Take(1));
        await userManager.AddToRolesAsync(users[2], roles.Select(r => r.Name)!.Take(1));
    }

}

using back.Contexts;
using back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace back.Extensions;

public static class ProgramExtensions
{
    public static void AddDatabaseAndEntities(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DatabaseConnection");

        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentity<User, Role>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders()
            .AddRoles<Role>();
    }
}

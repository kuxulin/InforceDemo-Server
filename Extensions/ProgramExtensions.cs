using back.Contexts;
using back.Models;
using back.Models.Consts;
using back.Repositories;
using back.Repositories.Interfaces;
using back.Services;
using back.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUrlService, UrlService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUrlRepository, UrlRepository>();
    }

    public static void AddAuthenticationConfigurations(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });
    }

    public static void AddPolicyAuthentication(this IServiceCollection services)
    {
        services.AddAuthorization(_ =>
        {
            _.AddPolicy(UserPolicy.PARTNER_ADMIN, p => p.RequireRole("Partner","Admin"));
            _.AddPolicy(UserPolicy.ADMIN, p => p.RequireRole("Admin"));
        });
    }
}

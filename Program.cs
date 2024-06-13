using back.Extensions;

namespace back;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDatabaseAndEntities();
        builder.Services.AddControllers();
        builder.Services.AddServices();
        builder.Services.AddRepositories();

        builder.Services.AddAuthenticationConfigurations();
        builder.Services.AddPolicyAuthentication();

        builder.Services.AddCors(options => options.AddPolicy("default", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseCors("default");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.SeedData();

        app.Run();
    }
}

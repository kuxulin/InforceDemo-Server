using back.Extensions;

namespace back;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddDatabaseAndEntities();
        builder.Services.AddControllers();
        builder.Services.AddServices();
        builder.Services.AddAuthenticationConfigurations();

        builder.Services.AddCors(options => options.AddPolicy("default", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseCors("default");

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapRazorPages();
        app.MapControllers();

        await app.SeedData();

        app.Run();
    }
}

using Core.Entities.Identity;
using demo.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Identity;

namespace demo.Helper;

public static class ConfigureMiddlewares
{
    public static async Task<WebApplication> ConfigureMiddlewaresAsync(this WebApplication app)
    {

        #region Apply all migrations & Data Seeding

        var services = app.Services.CreateScope().ServiceProvider;
        var context = services.GetRequiredService<StoreDbContext>();
        var identityContext = services.GetRequiredService<StoreIdentityDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

        try
        {
            await context.Database.MigrateAsync();
            await StoreDbContextSeed.SeedAsync(context);

            await identityContext.Database.MigrateAsync();
            await StoreIdentityDbContextSeeder.SeedAppUserAsync(userManager);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"There are problems during apply migrations!");
        }

        #endregion


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStatusCodePagesWithRedirects("/error/{0}");

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();

        app.UseMiddleware<ExceptionMiddleware>();


        app.MapControllers();

        return app;
    }
}

using demo.Helper;

namespace demo;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        DependancyInjection.AddDependancy(builder.Services, builder.Configuration);

        var app = builder.Build();

        await app.ConfigureMiddlewaresAsync();

        app.Run();
    }


    //// Method to register DbContexts dynamically from the assembly
    //private static void RegisterDbContextsFromAssembly(IServiceCollection services, Assembly assembly, IConfiguration configuration)
    //{
    //    // Scan for types that inherit from DbContext
    //    var dbContextTypes = assembly.GetTypes()
    //        .Where(type => type.IsSubclassOf(typeof(DbContext)))
    //        .ToList();

    //    foreach (var dbContextType in dbContextTypes)
    //    {
    //        // Using reflection to add DbContext type dynamically
    //        var dbContextOptionsType = typeof(DbContextOptions<>).MakeGenericType(dbContextType);

    //        services.Add(new ServiceDescriptor(dbContextType, provider =>
    //        {
    //            // Create options for the DbContext
    //            var optionsBuilder = (DbContextOptionsBuilder)Activator.CreateInstance(typeof(DbContextOptionsBuilder<>).MakeGenericType(dbContextType));
    //            optionsBuilder.UseSqlServer(configuration.GetConnectionString("sqlserver"));

    //            return Activator.CreateInstance(dbContextType, optionsBuilder.Options);
    //        }, ServiceLifetime.Scoped));
    //    }
    //}

}

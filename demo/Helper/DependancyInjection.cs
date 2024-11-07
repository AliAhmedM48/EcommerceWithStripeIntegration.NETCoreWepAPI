using Core;
using Core.Entities.Identity;
using Core.Mapping.Auth;
using Core.Mapping.Carts;
using Core.Mapping.Orders;
using Core.Mapping.Products;
using Core.Services.Contracts;
using Core.Services.Contracts.Carts;
using Core.Services.Contracts.Orders;
using Core.Services.Contracts.Products;
using demo.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Data;
using Repository.Identity;
using Service;
using StackExchange.Redis;
using System.Text;

namespace demo.Helper;

public static class DependancyInjection
{
    public static IServiceCollection AddDependancy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBuiltInService();
        services.AddSwaggerService();
        services.AddDbContextService(configuration);
        services.AddUserDefinedService();
        services.AddAutoMapperService(configuration);
        services.ConfigureInvalidModelStateResponseService();
        services.AddRedisService(configuration);
        services.AddIdentityService();
        services.AddAuthenticationService(configuration);
        return services;
    }

    private static IServiceCollection AddBuiltInService(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }

    private static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("sqlserver"));
        });

        services.AddDbContext<StoreIdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("sqlserverIdentity"));
        });



        return services;
    }

    private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
    {
        //// Load types from the current assembly or another assembly dynamically
        //var infrastructureAssembly = Assembly.Load("Repository");
        //RegisterDbContextsFromAssembly(builder.Services, infrastructureAssembly, builder.Configuration);

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();

        //builder.Services.AddScoped<IRepository<Product, int>, Repository<Product, int>>();
        //builder.Services.AddScoped<IRepository<ProductBrand, int>, Repository<ProductBrand, int>>();
        //builder.Services.AddScoped<IRepository<ProductType, int>, Repository<ProductType, int>>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }

    private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(m => m.AddProfile(new ProductProfile(configuration)));
        services.AddAutoMapper(m => m.AddProfile(new CartProfile()));
        services.AddAutoMapper(m => m.AddProfile(new AuthProfile()));
        services.AddAutoMapper(m => m.AddProfile(new OrderProfile(configuration)));
        return services;
    }

    private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = (actionContext) =>
            {
                var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                .SelectMany(p => p.Value.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

                var response = new ApiValidationErrorResponse() { Errors = errors };

                return new BadRequestObjectResult(response);
            };
        }
              ); return services;
    }

    private static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var connection = configuration.GetConnectionString("redis");
            Console.WriteLine($"====XXXXXXXXX===> {connection}");

            return ConnectionMultiplexer.Connect(connection);
        });
        return services;
    }

    private static IServiceCollection AddIdentityService(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<StoreIdentityDbContext>();
        return services;
    }

    private static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });
        return services;
    }
}

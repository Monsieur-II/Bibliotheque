using System.Security.Claims;
using System.Text;
using AuthLab.Api.Data;
using AuthLab.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace AuthLab.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Defualt"));
        dataSourceBuilder.UseJsonNet(); // or `dataSourceBuilder.EnableDynamicJson();` for System.Text.Json mappings
        
        var npgsqlDataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(npgsqlDataSource);
        });
    }
    
    public static IServiceCollection AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var bearerConfig = configuration.GetSection(nameof(BearerAuthConfig)).Get<BearerAuthConfig>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = bearerConfig!.Issuer,
                    ValidAudience = bearerConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bearerConfig.SigningKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    //ValidateLifetime = true,
                    RequireExpirationTime = false
                };
                x.SaveToken = true;
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        await ProcessUserDataAsync(ctx);
                    }
                };
            });

        return services;
    }

    private static async Task ProcessUserDataAsync(TokenValidatedContext ctx)
    {
        var phoneNumber = ctx.Principal?.FindFirst(x => x.Type == ClaimTypes.MobilePhone)?.Value ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            ctx.Fail("Unknown user");

        var dbContext = ctx.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

        var user = await dbContext!.Customers.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

        var claims = new List<Claim>
        {
            new(ClaimTypes.MobilePhone, phoneNumber),
            new(ClaimTypes.Authentication, "Bearer"),
            new(ClaimTypes.UserData, user.Serialize())
        };

        var identity = new ClaimsIdentity(claims, "AuthLab.Api");
        
        ctx.Principal?.AddIdentity(identity);
    }
}

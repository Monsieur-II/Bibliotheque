using AuthLab.Api.Authentication;
using AuthLab.Api.Data;
using AuthLab.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = $@"Enter '[Bearer]' [space] and then your token in the text input below.<br/>
                      Example: '{"Bearer"} 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });
});

services.AddApplicationDbContext(config);

services.AddRouting(x => x.LowercaseUrls = true);

//Auth
services.AddBearerAuthentication(config);

services.AddAuthorization();
services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
services.AddSingleton<IAuthorizationPolicyProvider,PermissionPolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var scope = app.Services.CreateScope();
var scopedServiceProvider = scope.ServiceProvider;
var context = scopedServiceProvider.GetRequiredService<ApplicationDbContext>();

// // Ensure database is created
await context.Database.EnsureCreatedAsync();

var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

if (pendingMigrations.Any())
{
    await context.Database.MigrateAsync(); //NB: This also calls EnsureCreated under the hood
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


await app.RunAsync();

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynamoDb.Api.Data;
using DynamoDb.Api.Data.Repositories;
using DynamoDb.Api.Data.Repositories.DynamoDb;
using DynamoDb.Api.Data.Repositories.Postgres;
using DynamoDb.Api.Extensions;
using DynamoDb.Api.Options;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});

services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Postgres
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("default")));

// DynamoDB

services.AddDynamoDb(builder.Configuration);


// DI
services.AddScoped<IDynamoDBContext, DynamoDBContext>();
services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<IDynamoCustomerRepository, DynamoCustomerRepository>();

services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(webapp => webapp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
    }));
}

var scope = app.Services.CreateScope();
var scopedServiceProvider = scope.ServiceProvider;
var context = scopedServiceProvider.GetRequiredService<ApplicationDbContext>();

// // Ensure database is created
await context.Database.EnsureCreatedAsync();

var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

if (pendingMigrations.Any())
{
    await context.Database.MigrateAsync();
}

app.CreateDynamoDbTables(app.Configuration);


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.MapControllers();

app.Run();

using ElasticSearch.Api.Extensions;
using ElasticSearch.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.
services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
services.AddControllers();
services.AddEndpointsApiExplorer();

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

services.AddSwaggerGen();

//Logging
services.AddSerilogService(config);

services.AddElasticSearchConfiguration(config);

services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<ICartItemService, CartItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Starting application");

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();

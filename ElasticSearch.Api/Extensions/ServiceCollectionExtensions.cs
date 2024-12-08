using ElasticSearch.Api.Entities;
using ElasticSearch.Api.Options;
using Nest;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;

namespace ElasticSearch.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSerilogService(this IServiceCollection services, IConfiguration config)
    {
        services.AddSerilog(new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Application", "ElasticSearch.Api")
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Graylog(new GraylogSinkOptions
            {
                HostnameOrAddress = "localhost",
                Port = 12201,
                MinimumLogEventLevel = LogEventLevel.Debug,
                Facility = "ElasticSearch.Api",
                TransportType = TransportType.Udp
            })
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"ElasticSearchApiLogs-{DateTime.UtcNow:yyyy.MM}",
                NumberOfReplicas = 1,
                NumberOfShards = 2
            })
            .CreateLogger());
    }
    
    public static IServiceCollection AddElasticSearchConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ElasticConfig>(c => configuration.GetSection(nameof(ElasticConfig)).Bind(c));

        var config = configuration.GetSection(nameof(ElasticConfig)).Get<ElasticConfig>();

        if (config!.Hosts is null || config.Hosts.Count == 0)
        {
            throw new Exception("No ElasticConfiguration provided!!!");
        }

        var host = config.Hosts[0];
        var index = config.Hosts[0].Indexes[0];

        var settings = new ConnectionSettings(new Uri(host.Url))
            .PrettyJson()
            .DefaultIndex(index.Index);
        
        settings.AddCustomerMappings(index.Index);
        
        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);
        
        CreateCustomerIndex(client, index.Index);
        
        return services;
    }
    
    private static void AddCustomerMappings(this ConnectionSettings settings, string indexName)
    {
        settings.DefaultMappingFor<Customer>(x =>
            x.IdProperty(c => c.Id)
            .Ignore(c => c.Address)
            .Ignore(c => c.City)
            .Ignore(c => c.Country)
            .Ignore(c => c.State)
            .Ignore(c => c.ZipCode)
            .IndexName(indexName)
        );
    }

    private static void CreateCustomerIndex(IElasticClient client, string indexName)
    {
        client.Indices.Create(indexName, i => i
            .Map<Customer>(x => x.
                AutoMap()));
    }
}

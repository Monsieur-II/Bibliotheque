using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynamoDb.Api.Options;
using Microsoft.Extensions.Options;

namespace DynamoDb.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDynamoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var dynamoDbConfig = configuration.GetSection(nameof(DynamoDbConfig)).Get<DynamoDbConfig>();

        var clientConfig = new AmazonDynamoDBConfig();

        if (dynamoDbConfig!.LocalMode)
        {
            clientConfig.ServiceURL = dynamoDbConfig.LocalServiceUrl; // Default is http://localhost:8000
        }

        var client = new AmazonDynamoDBClient("fake", "fake", clientConfig);

        // NB: client could also be instantiated as follows:
        // var client = new AmazonDynamoDBClient(clientConfig);
        // But with this approach, you will need to set the AWS_ACCESS_KEY_ID and AWS_SECRET_ACCESS_KEY environment variables and it will be picked from there.

        services.AddSingleton<IAmazonDynamoDB>(client); // Register the client
        services.AddSingleton<IDynamoDBContext, DynamoDBContext>(); // Register the context.

        services.Configure<DynamoTablesConfig>(options => 
            configuration.GetSection(nameof(DynamoTablesConfig)).Bind(options));
        
    }
}

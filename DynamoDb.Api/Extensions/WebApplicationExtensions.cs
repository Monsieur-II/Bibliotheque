using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDb.Api.Options;

namespace DynamoDb.Api.Extensions;

public static class WebApplicationExtensions
{
    public static async void CreateDynamoDbTables(this WebApplication app, IConfiguration configuration)
    {
        var dynamoDb = app.Services.GetRequiredService<IAmazonDynamoDB>();

        var tablesConfig = configuration.GetSection(nameof(DynamoTablesConfig)).Get<DynamoTablesConfig>();

        var tables = await dynamoDb.ListTablesAsync();
        if (!tables.TableNames.Contains(tablesConfig!.CustomerTableName))
        {
            var request = new CreateTableRequest
            {
                TableName = tablesConfig.CustomerTableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new("Id", ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new("Id", KeyType.HASH)
                },
                ProvisionedThroughput = new ProvisionedThroughput(10, 10)
            };

            await dynamoDb.CreateTableAsync(request);
        }
    }
}

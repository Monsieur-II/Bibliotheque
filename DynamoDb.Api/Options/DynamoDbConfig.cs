namespace DynamoDb.Api.Options;

public class DynamoDbConfig
{
    public bool LocalMode { get; set; }
    public string LocalServiceUrl { get; set; }
}

public class DynamoTablesConfig
{
    public string CustomerTableName { get; set; }
}

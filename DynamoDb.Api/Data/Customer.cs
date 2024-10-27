using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDb.Api.Data;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DynamoDBHashKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    [DynamoDBProperty, DynamoDBGlobalSecondaryIndexRangeKey]
    public string Name { get; set; }
    [DynamoDBProperty]
    public string? Email { get; set; }
    [DynamoDBProperty]
    public string? PhoneNumber { get; set; }
    [DynamoDBProperty]
    public bool IsDeleted { get; set; }
    [DynamoDBProperty]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

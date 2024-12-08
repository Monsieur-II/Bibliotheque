using Nest;

namespace ElasticSearch.Api.Entities;

public class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    [Ignore]
    public string CreatedAt { get; set; }
    [Ignore]
    public string UpdatedAt { get; set; }
}

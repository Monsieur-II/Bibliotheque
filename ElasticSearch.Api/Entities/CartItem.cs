using Nest;

namespace ElasticSearch.Api.Entities;

public class CartItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string CustomerId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    [Ignore]
    public string CreatedAt { get; set; }
    [Ignore]
    public string UpdatedAt { get; set; }
}

using Nest;

namespace ElasticSearch.Api.Entities;

public class CartItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string CustomerId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    [Ignore]
    public DateTime UpdatedAt { get; set; } =  DateTime.UtcNow;
}

using Nest;

namespace ElasticSearch.Api.Entities;

public class Cart
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string CustomerId { get; set; }
    public List<CartItem> Items { get; set; }
    [Ignore]
    public string CreatedAt { get; set; }
    [Ignore]
    public string UpdatedAt { get; set; }
}

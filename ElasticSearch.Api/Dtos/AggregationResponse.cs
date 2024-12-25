using ElasticSearch.Api.Entities;

namespace ElasticSearch.Api.Dtos;

public class CartAggregationResponse
{
    public double MaxQuantity { get; set; }
    public CartItemResponse? TopCartItem { get; set; }
}

public class GroupedAggregationResponse
{
    public string Key { get; set; }
    public double MaxQuantity { get; set; }
}

public class CartItemResponse
{
    public string? Id { get; set; }
    public string? CustomerId { get; set; }
    public string? ProductId { get; set; }
    public int Quantity { get; set; }
}

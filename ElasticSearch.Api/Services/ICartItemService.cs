using ElasticSearch.Api.Controllers;
using ElasticSearch.Api.Dtos;
using ElasticSearch.Api.Entities;

namespace ElasticSearch.Api.Services;

public interface ICartItemService
{
    Task<bool> AddAsync(AddCartItemRequest request);
    Task<CartAggregationResponse?> GetMaxQuantityAsync(string customerId);
    Task<List<GroupedAggregationResponse>> GetMaxQuantityByProductsAsync(string customerId);
}

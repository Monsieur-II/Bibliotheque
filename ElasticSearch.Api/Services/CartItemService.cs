using System.Data;
using ElasticSearch.Api.Controllers;
using ElasticSearch.Api.Dtos;
using ElasticSearch.Api.Entities;
using ElasticSearch.Api.Extensions;
using Mapster;
using Nest;

namespace ElasticSearch.Api.Services;

public class CartItemService(IElasticClient elasticClient,
    ILogger<CartItemService> logger) : ICartItemService
{
    public async Task<bool> AddAsync(AddCartItemRequest request)
    {
        logger.LogInformation("Adding cart item with payload: {Payload}", request.Serialize());
        
        var cartItem = request.Adapt<CartItem>();
        
        var response = await elasticClient.IndexDocumentAsync(cartItem);
        
        return response.IsValid;
    }

    public async Task<CartAggregationResponse?> GetMaxQuantityAsync(string customerId)
    {
        var searchRequest = new SearchDescriptor<CartItem>()
            .Query(x => x
                .Bool(b => b.
                    Must(
                        m => m
                            .Term(r => r
                                .Field(c => c.CustomerId.Suffix("keyword"))
                                .Value(customerId))
                       // , 
                        // r => r.
                        //     DateRange(d => d
                        //         .Field(c => c.CreatedAt)
                        //         .LessThanOrEquals(DateTime.UtcNow))
                    )
                )
            )
            .Size(0) // This top-level size only applies to the query results and not the aggregation results. In this case, we are only interested in the aggregation results.
            .Aggregations(x => x.
                Max("Max_quantity", m => m.
                    Field(c => c.Quantity))
                .TopHits("Top_cart_item", t => t
                    .Sort(s => s.
                        Descending(c => c.Quantity))
                    .Size(1) // This size is the number of hits to return from the top hits aggregation. NB: There could be multiple hits with the same max quantity. In this case, we only want one hit.
                )
            );
        
        var searchResponse = await elasticClient.SearchAsync<CartItem>(searchRequest);
        
        var maxQuantity = searchResponse.Aggregations.Max("Max_quantity")?.Value;
        var topCartItem = searchResponse.Aggregations.TopHits("Top_cart_item")?.Documents<CartItem>().FirstOrDefault();
        
       var response = new CartAggregationResponse
       {
           MaxQuantity = maxQuantity ?? 0,
           TopCartItem = topCartItem.Adapt<CartItemResponse>()
       };
       
         return response;
    }

    public async Task<List<GroupedAggregationResponse>> GetMaxQuantityByProductsAsync(string customerId)
    {
        var searchRequest = new SearchDescriptor<CartItem>()
            .Query(q => q
                .Bool(b => b
                    .Must(
                        m => m
                            .Term(t => t.Field(f => f.CustomerId.Suffix("keyword"))
                                .Value(customerId))
                       // , 
                       //  n => n.DateRange(d => d.LessThanOrEquals(DateTime.UtcNow))
                        )
                ))
            .Size(0) // no top-level based docs, only aggregations
            .Aggregations(a => a
                .Terms("productId", p => p
                    .Field(f => f.ProductId.Suffix("keyword"))
                    .Aggregations(ag => ag
                        .Max("max_quantity", m => m
                            .Field(f => f.Quantity)
                        )
                    )
                )
            );
        
        var searchResponse = await elasticClient.SearchAsync<CartItem>(searchRequest);

        if (!searchResponse.IsValid)
            throw new NoNullAllowedException();

        var response = new List<GroupedAggregationResponse>();

        var productGroups = searchResponse.Aggregations.Terms("productId")?.Buckets;
        
        if (productGroups == null)
            return response;

        foreach (var group in productGroups)
        {
            response.Add(new GroupedAggregationResponse
            {
                Key = group.Key,
                MaxQuantity = group.Max("max_quantity")?.Value ?? 0
            });
        }
        
        return response;
    }
}

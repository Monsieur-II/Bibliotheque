using System.ComponentModel.DataAnnotations;
using ElasticSearch.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearch.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CartItemsController(ICartItemService cartItemService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddCart(AddCartItemRequest request)
    {
        var response = await cartItemService.AddAsync(request);
        
        if (!response)
            return BadRequest();
        
        return Ok();
    }
    
    [HttpGet("max-quantity/{customerId}")]
    public async Task<IActionResult> GetMaxQuantity(string customerId)
    {
        var response = await cartItemService.GetMaxQuantityAsync(customerId);
        
        return Ok(response);
    }
    
    [HttpGet("grouped-max-quantity/{customerId}")]
    public async Task<IActionResult> GetGroupedMaxQuantity(string customerId)
    {
        var response = await cartItemService.GetMaxQuantityByProductsAsync(customerId);
        
        return Ok(response);
    }
}

public class AddCartItemRequest
{
    public string CustomerId { get; set; }
    public string ProductId { get; set; }
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

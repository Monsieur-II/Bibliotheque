using ElasticSearch.Api.Dtos;
using ElasticSearch.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddCustomer(AddCustomerRequest request)
    {
        var response = await customerService.AddAsync(request);
        
        if (!response)
            return BadRequest();
        
        return Ok();
    }
    
    [HttpGet("id")]
    public async Task<IActionResult> GetCustomer(string id)
    {
        var response = await customerService.GetAsync(id);
        
        if (response == null)
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCustomers(BaseFilter filter)
    {
        var response = await customerService.GetAllAsync(filter);
        
        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCustomers([FromQuery] BaseFilter filter)
    {
        var response = await customerService.SearchAsync(filter);
        
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer(UpdateCustomerRequest request)
    {
        var response = await customerService.UpdateAsync(request);
        
        if (!response)
            return BadRequest();
        
        return Ok();
    }
    
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        var response = await customerService.DeleteAsync(id);
        
        if (!response)
            return BadRequest();
        
        return Ok();
    }
}

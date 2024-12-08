using DynamoDb.Api.Data.Repositories.DynamoDb;
using DynamoDb.Api.Data.Repositories.Postgres;
using DynamoDb.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDb.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomersController(ICustomerRepository customerRepository, IDynamoCustomerRepository dynamoCustomerRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddCustomer(AddCustomerRequest customer)
    {
        var response = await customerRepository.AddCustomer(customer);
        
        return Ok(response);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var response = await dynamoCustomerRepository.GetCustomers();
        
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(string id)
    {
        var response = await dynamoCustomerRepository.GetAsync(id);
        
        if (response == null)
            return NotFound();
        
        return Ok(response);
    }
    
    [HttpGet("email")]
    public async Task<IActionResult> GetCustomerByEmail([FromQuery] string email)
    {
        var response = await dynamoCustomerRepository.GetCustomerByEmailAsync(email);
        
        if (response == null)
            return NotFound();
        
        return Ok(response);
    }
    
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        var response = await customerRepository.SoftDeleteCustomer(id);
        
        return Ok(response);
    }
}

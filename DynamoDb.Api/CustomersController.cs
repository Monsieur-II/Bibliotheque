using DynamoDb.Api.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDb.Api;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomersController(ICustomerRepository customerRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var response = await customerRepository.GetCustomers();
        
        return Ok(response);
    }
}

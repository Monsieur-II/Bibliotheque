using AuthLab.Api.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthLab.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[]
        {
            new Customer { FullName = "John Doe", PhoneNumber = "555-1234" },
            new Customer { FullName = "Jane Doe", PhoneNumber = "555-5678" }
        });
    }
}

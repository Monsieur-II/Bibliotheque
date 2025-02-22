using AuthLab.Api.CommonConstants;
using AuthLab.Api.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthLab.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(AuthenticationSchemes = $"{AuthenticationSchemes.Bearer}, {AuthenticationSchemes.Basic}, {AuthenticationSchemes.Private}")]
public class StaffController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[]
        {
            new Staff { FullName = "John Doe", PhoneNumber = "555-1234" },
            new Staff { FullName = "Jane Doe", PhoneNumber = "555-5678" }
        });
    }
}

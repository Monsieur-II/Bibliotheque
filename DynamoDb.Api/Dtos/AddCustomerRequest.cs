using System.ComponentModel.DataAnnotations;

namespace DynamoDb.Api.Dtos;

public class AddCustomerRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}

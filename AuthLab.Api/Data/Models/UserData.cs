namespace AuthLab.Api.Data.Models;

public class UserData
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string> Permissions { get; set; } = [];
}

namespace AuthLab.Api.Data.Models;

public class Staff : BaseEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> Permissions { get; set; } = [];
}

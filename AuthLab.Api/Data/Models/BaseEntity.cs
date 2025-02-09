namespace AuthLab.Api.Data.Models;

public abstract class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

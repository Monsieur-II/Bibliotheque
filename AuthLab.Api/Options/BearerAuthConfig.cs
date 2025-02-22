namespace AuthLab.Api.Options;

public class BearerAuthConfig
{
    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string SigningKey { get; set; } = null!;
}

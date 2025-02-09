namespace AuthLab.Api.Options;

public class BearerAuthConfig
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string SigningKey { get; set; }
}

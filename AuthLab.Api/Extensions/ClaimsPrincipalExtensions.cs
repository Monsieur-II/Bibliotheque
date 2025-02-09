using System.Security.Claims;
using AuthLab.Api.Data.Models;

namespace AuthLab.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static UserData GetUserData(this ClaimsPrincipal principal)
    {
        var identity = principal.Identities.FirstOrDefault(x => x.AuthenticationType == "AuthLab.Api");

        if (identity is null)
            return new UserData();

        var userData = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;

        if (string.IsNullOrWhiteSpace(userData))
            return new UserData();

        var user = userData.Deserialize<UserData>();

        return user ?? new UserData();
    }
}

using Microsoft.AspNetCore.Authorization;

namespace AuthLab.Api.Authentication;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
    {
        Policy = permission;
    }
}

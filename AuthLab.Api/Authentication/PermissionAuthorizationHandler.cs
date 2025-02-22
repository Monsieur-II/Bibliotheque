using AuthLab.Api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace AuthLab.Api.Authentication;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var user = context.User.GetUserData();

        if (user.Permissions.Exists(x => x == requirement.Permission))
        {
            context.Succeed(requirement); // if this is not called before returning, authorization will fail implicitly
            
            return;
        }

        await Task.CompletedTask;
    }
}

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}

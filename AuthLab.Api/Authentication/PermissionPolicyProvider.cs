using AuthLab.Api.CommonConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AuthLab.Api.Authentication;

/// <summary>
/// With this, policies do not need to be manually registered in the service collection.
/// </summary>
public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }
    
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName); // this returns the policy if it exists in the policy cache (which actually is a <string, AuthorizationPolicy> dictionary), otherwise null

        if (policy is not null)
            return policy;
        
        // policy not found, create a new one
        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Fymate.Domain.Base.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace Fymate.Web.Services
{
    public class IsOwnerAuthorizationHandler : AuthorizationHandler<IsOwnerAuthorizationRequirement, IHasOwner>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerAuthorizationRequirement requirement, IHasOwner resource)
        {   

            if (context.User.FindFirst(JwtRegisteredClaimNames.Sub).Value == resource.OwnerID)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class IsOwnerAuthorizationRequirement : IAuthorizationRequirement { }
}
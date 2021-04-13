using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Fymate.Domain.Base.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Fymate.Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Fymate.Web.Services
{
    public class IsExistingProfileAuthorizationHandler : AuthorizationHandler<IsExistingProfileRequirement, string>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public IsExistingProfileAuthorizationHandler(ILoggerFactory loggerFactory, UserManager<ApplicationUser> userManager)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().FullName);
            _userManager = userManager;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsExistingProfileRequirement requirement, string resource)
        {
            //Compare Auth values
            var sub = context.User.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            var u = await _userManager.FindByIdAsync(sub);
            Console.WriteLine(sub);
            Console.WriteLine(u?.UserName);
            if (u == null)
                context.Fail();

            //Compare context values
            if (u?.UserName == resource)
            {
                context.Succeed(requirement);
            }
        }
    }

    public class IsExistingProfileRequirement : IAuthorizationRequirement { }
}
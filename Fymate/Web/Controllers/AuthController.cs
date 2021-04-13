using Fymate.Core.Base.Models;
using Fymate.Core.Concrete.Identity.Commands;
using Fymate.Core.Concrete.Models;
using Fymate.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Fymate.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ApiControllerBase
    {

        private readonly IAuthorizationService _authService;

        public AuthController(IAuthorizationService authService)
        {
            _authService = authService;
        }

        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> RegisterUser(RegisterUserCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Logins users, and returns JWT token on successfull auth
        /// </summary>
        /// <param name="command"></param>
        /// <returns>
        /// 200, JWT token - if successful
        /// 401 - if email, password pair not found
        /// </returns>
        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<ActionResult<JWTAuthorizationResult>> LoginUser(LoginUserCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.Succeeded)
                return new OkObjectResult(result.Token);
            return new UnauthorizedResult();
        }

        [HttpPost("LogoutUser")]
        public async Task<ActionResult<bool>> LogoutUser(LogoutUserCommand command)
        {
            //TODO: unneeded in JWT, logout is handled on client side
            return await Mediator.Send(command);
        }

        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult<bool>> ConfirmEmail(ConfirmEmailCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPatch("ChangeEmail")]
        public async Task<ActionResult<bool>> ChangeEmail(ChangeEmailCommand command)
        {
            var authResult = await _authService.AuthorizeAsync(User, command.UserName, new IsExistingProfileRequirement());
            if (authResult.Succeeded == false)
                return new UnauthorizedResult();
            return await Mediator.Send(command);
        }

        [HttpPatch("ChangePassword")]
        public async Task<ActionResult<bool>> ChangePassword(ChangePasswordCommand command)
        {
            var authResult = await _authService.AuthorizeAsync(User, command.UserName, new IsExistingProfileRequirement());
            if (authResult.Succeeded == false)
                return new UnauthorizedResult();
            return await Mediator.Send(command);
        }

        [HttpPatch("ResetPassword")]
        public async Task<ActionResult<bool>> ResetPassword(ResetPasswordCommand command)
        {
            var authResult = await _authService.AuthorizeAsync(User, command.UserName, new IsExistingProfileRequirement());
            if (authResult.Succeeded == false)
                return new UnauthorizedResult();
            return await Mediator.Send(command);
        }
    }
}

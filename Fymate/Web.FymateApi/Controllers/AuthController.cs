using Core.Base.Models;
using Core.Concrete.Identity.Commands;
using Core.Concrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Web.FymateApi.Controllers
{
    [Route("api/[controller]")]
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
        [Authorize]
        public async Task<ActionResult<bool>> LogoutUser(LogoutUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("ConfirmEmail")]
        [Authorize]
        public async Task<ActionResult<bool>> ConfirmEmail(ConfirmEmailCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("ChangeEmail")]
        [Authorize]
        public async Task<ActionResult<bool>> ChangeEmail(ChangeEmailCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPatch("ChangePassword")]
        [Authorize]
        public async Task<ActionResult<bool>> ChangePassword(ChangePasswordCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPatch("ResetPassword")]
        [Authorize]
        public async Task<ActionResult<bool>> ResetPassword(ResetPasswordCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}

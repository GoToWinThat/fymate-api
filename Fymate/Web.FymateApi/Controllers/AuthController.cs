using Core.UseCases.Identity.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.FymateApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ApiControllerBase
    {
        [Route("RegisterUser")]
        public async Task<ActionResult<bool>> RegisterUser(RegisterUserCommand command)
        {
            return await Mediator.Send(command);
        }
        [Route("LoginUser")]
        public async Task<ActionResult<bool>> LoginUser(LoginUserCommand command)
        {
            return await Mediator.Send(command);
        }
        [Route("LogoutUser")]
        public async Task<ActionResult<bool>> LogoutUser(LogoutUserCommand command)
        {
            return await Mediator.Send(command);
        }
        [Route("ConfirmEmail")]
        public async Task<ActionResult<bool>> ConfirmEmail(ConfirmEmailCommand command)
        {
            return await Mediator.Send(command);
        }
        [Route("ChangeEmail")]
        public async Task<ActionResult<bool>> ChangeEmail(ChangeEmailCommand command)
        {
            return await Mediator.Send(command);
        }
        [Route("ChangePassword")]
        public async Task<ActionResult<bool>> ChangePassword(ChangePasswordCommand command)
        {
            return await Mediator.Send(command);
        }
        [Route("ResetPassword")]
        public async Task<ActionResult<bool>> ResetPassword(ResetPasswordCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}

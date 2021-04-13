using Fymate.Core.Base.Interfaces;
using Fymate.Core.Concrete.Profiles.Commands;
using Fymate.Core.Concrete.Profiles.Queries;
using Fymate.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fymate.Web.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Fymate.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ApiControllerBase
    {

        private readonly IAuthorizationService _authorizationService;
        private readonly IApplicationDbContext _db;
        private readonly ICurrentUserService _cus;


        public ProfileController(IAuthorizationService authorizationService, IApplicationDbContext db)
        {
            _authorizationService = authorizationService;
            _db = db;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ProfilesVm>> GetProfiles()
        {
            return await Mediator.Send(new GetProfilesQuery());
        }

        [HttpPost("{profileID}")]
        public async Task<ActionResult> PostProfile(int profileID, ModifyProfileCommandPayload command)
        {
            //TODO: somehow reuse this? we make same call in profile service 
            Profile profile = _db.Profiles.First(x => x.Id == profileID);
            //Authorize
            var authResult = await _authorizationService.AuthorizeAsync(User, profile, new IsOwnerAuthorizationRequirement());
            if (authResult.Succeeded == false)
                return new UnauthorizedResult();

            command.id = profileID;
            if (await Mediator.Send(command))
                return new OkResult();
            throw new Exception("Could not process request");
        }
    }
}

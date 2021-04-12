using Core.Base.Interfaces;
using Core.Concrete.Profiles.Commands;
using Core.Concrete.Profiles.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.FymateApi.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Web.FymateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ApiControllerBase
    {

        private readonly IAuthorizationService _authorizationService;
        private readonly IApplicationDbContext _db;


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
            //Now this is bad because we do the same fetch in our Service
            //But we need to do this here, because our services
            //dont have a full request information like Controllers do
            //[Authorize] cannot determine a "resource" to do operations on
            //so IAuthorizationService neede to be injected. I welcome better proposals 
            //This prime candidate for refactor.
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

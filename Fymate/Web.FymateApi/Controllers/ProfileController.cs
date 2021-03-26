using Core.UseCases.Profiles.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.FymateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ApiControllerBase
    {
        [HttpGet]
        [Route("Profiles")]
        public async Task<ActionResult<ProfilesVm>> GetProfiles()
        {
            return await Mediator.Send(new GetProfilesQuery());
        }
    }
}

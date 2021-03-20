using Core.UseCases.Profiles.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.FymateApi.Controllers
{
    public class ProfileController :ApiControllerBase
    {
        [HttpGet]
        [Route("Profiles")]
        public async Task<ActionResult<ProfilesVm>> GetProfiles()
        {
            return await Mediator.Send(new GetProfilesQuery());
        }
    }
}

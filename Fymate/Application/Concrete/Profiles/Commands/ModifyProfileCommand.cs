using Fymate.Core.Base.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Fymate.Core.Concrete.Profiles.Commands
{
    public class ModifyProfileCommandPayload : IRequest<bool>
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class ModifyProfileCommandHandler : IRequestHandler<ModifyProfileCommandPayload, bool>
    {
        private readonly IProfileService _profileService;

        public ModifyProfileCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<bool> Handle(ModifyProfileCommandPayload request, CancellationToken cancellationToken)
        {
            var result = await _profileService.ModifyProfileAsync(request.id, request.description);
            return result.Succeeded;
        }
    }
}

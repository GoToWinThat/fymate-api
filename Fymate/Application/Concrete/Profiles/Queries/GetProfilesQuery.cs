using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fymate.Core.Base.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fymate.Core.Concrete.Profiles.Queries
{
    public class GetProfilesQuery : IRequest<ProfilesVm> { }
    public class GetProfilesQueryHandler : IRequestHandler<GetProfilesQuery, ProfilesVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetProfilesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProfilesVm> Handle(GetProfilesQuery request, CancellationToken cancellationToken)
        {
            return new ProfilesVm
            {
                Profiles = await _context.Profiles.ProjectTo<ProfileDto>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.ProfileID)
                    .ToListAsync(cancellationToken)
            };
        }
    }
}

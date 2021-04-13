using Fymate.Core.Base.Mappings;
using Fymate.Domain.Entities;

namespace Fymate.Core.Concrete.Profiles.Queries
{
    public class ProfileDto : IMapFrom<Profile>
    {
        public int ProfileID { get; set; }
        public string ProfileDescription { get; set; }

        public static void Mapping(MappingProfile profile)
        {
            profile.CreateMap<Profile, ProfileDto>()
                .ForMember(dest => dest.ProfileID, source => source.MapFrom(s => s.Id))
                .ForMember(dest => dest.ProfileDescription, source => source.MapFrom(s => s.Description));
        }
    }
}
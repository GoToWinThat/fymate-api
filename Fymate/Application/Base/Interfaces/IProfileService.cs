using Fymate.Core.Base.Models;
using Fymate.Core.Concrete.Models;
using System.Threading.Tasks;

namespace Fymate.Core.Base.Interfaces
{
    public interface IProfileService
    {
        Task<Result> CreateProfileAsync(string applicationUserID);
        Task<Result> ModifyProfileAsync(int profileID, string description);
    }
}

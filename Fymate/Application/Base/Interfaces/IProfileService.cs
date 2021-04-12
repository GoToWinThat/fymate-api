using Core.Base.Models;
using Core.Concrete.Models;
using System.Threading.Tasks;

namespace Core.Base.Interfaces
{
    public interface IProfileService
    {
        Task<Result> CreateProfileAsync(string applicationUserID);
        Task<Result> ModifyProfileAsync(int profileID, string description);
    }
}

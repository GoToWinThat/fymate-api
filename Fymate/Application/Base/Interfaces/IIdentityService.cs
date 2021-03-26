using Core.Base.Models;
using System.Threading.Tasks;

namespace Core.Base.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<Result> LoginUserAsync(string email, string password);

        Task LogoutUserAsync(string userName);
        Task<Result> ChangePasswordAsync(string userName, string email, string oldPassword, string newPassword);
        Task<Result> ResetPasswordAsync(string userName, string email, string newPassword);
        Task<Result> ConfirmEmailAsync();
        Task<Result> ChangeEmailAsync(string userName, string email, string newEmail);
        Task<(Result Result, string UserId)> RegisterUserAsync(string email,string userName, string password);

        Task<Result> DeleteUserAsync(string userId);
    }
}

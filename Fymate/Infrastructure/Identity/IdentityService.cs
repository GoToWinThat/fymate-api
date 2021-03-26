using Core.Base.Interfaces;
using Core.Base.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
        }
        public async Task<bool> AuthorizeAsync(string userName, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == userName);
            

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }
        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public async Task<bool> IsInRoleAsync(string userName, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == userName);

            return await _userManager.IsInRoleAsync(user, role);
        }
        public async Task<(Result Result, string UserId)> RegisterUserAsync(string email, string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }
        public async Task<Result> LoginUserAsync(string email, string password)
        {
            SignInResult result = new SignInResult();
            if(_signInManager!=null)
            {
                result = await _signInManager.PasswordSignInAsync("Artur",password,true,false);
              var user = _userManager.Users.SingleOrDefault(u => u.Email == email);
                await AuthorizeAsync(user.UserName, "CanPurge");
            }

            if(result.Succeeded==true)
            {
                return Result.Success();
            }
            else
            {
                return Result.Failure(new string[] { "Wrong password" });
            }
        }

        public async Task LogoutUserAsync(string userName)
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Result> ConfirmEmailAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Result> ChangePasswordAsync(string userName,string email,string oldPassword,string newPassword)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
            };
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.ToApplicationResult();
        }

        public async Task<Result> ResetPasswordAsync(string userName, string email, string newPassword)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
            };
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user,token,newPassword);
            return result.ToApplicationResult();
        }

        public async Task<Result> ChangeEmailAsync(string userName, string email, string newEmail)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
            };
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = await _userManager.ChangeEmailAsync(user,newEmail,token);
            return result.ToApplicationResult();
        }
    }
}

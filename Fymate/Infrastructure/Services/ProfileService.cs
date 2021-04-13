using Fymate.Core.Base.Interfaces;
using Fymate.Core.Base.Models;
using Fymate.Core.Concrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Fymate.Domain.Entities;

namespace Fymate.Infrastructure.Identity
{
    public class ProfileService : IProfileService
    {
        private readonly IApplicationDbContext db;

        public ProfileService(IApplicationDbContext dbContext)
        {
            db = dbContext;
        }


        /// <summary>
        /// Creates profile for a given user account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> CreateProfileAsync(string applicationUserID)
        {

            await db.Profiles.AddAsync(new Profile()
            {
                OwnerID = applicationUserID,
                Description = "Welcome to my page",
            });

            //TODO: check if passing any CT is good
            await db.SaveChangesAsync(new System.Threading.CancellationToken());
            return Result.Success();
        }

        public async Task<Result> ModifyProfileAsync(int id, string description)
        {
            var profile = await db.Profiles.FirstAsync(x => x.Id == id);
            profile.Description = description;
            //TODO: check if passing any CT is good
            await db.SaveChangesAsync(new System.Threading.CancellationToken());
            return Result.Success();
        }
    }
}

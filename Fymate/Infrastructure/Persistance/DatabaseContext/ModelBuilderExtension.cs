using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.DatabaseContext
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            #region Profiles
            modelBuilder.Entity<Profile>().HasData(
                new Profile
                {
                    Id=1,
                    Description="First Developer"
                });
            #endregion
            #region Adverts
            modelBuilder.Entity<Advert>().HasData(
                new Advert
                {
                    Id = 1,
                    Description = "First Job",
                    ProfileId = 1
                });
            #endregion
        }
    }
}

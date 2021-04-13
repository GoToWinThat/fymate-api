using Fymate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fymate.Infrastructure.Persistance.Configuration
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasMany(p => p.Adverts)
                .WithOne(a => a.Profile)
                .HasForeignKey(a => a.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Description)
                .HasMaxLength(100);
        }
    }
}

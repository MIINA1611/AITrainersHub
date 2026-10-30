using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class PlatformCountryConfiguration : IEntityTypeConfiguration<PlatformCountry>
    {
        public void Configure(EntityTypeBuilder<PlatformCountry> builder)
        {
            builder.ToTable("PlatformCountries");
            builder.HasKey(pc => new { pc.PlatformId, pc.CountryId });

            builder.Property(pc => pc.Notes)
                .HasMaxLength(300);

            builder.HasOne(pc => pc.Platform)
                .WithMany(p => p.PlatformCountries)
                .HasForeignKey(pc => pc.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pc => pc.Country)
                .WithMany(c => c.PlatformCountries)
                .HasForeignKey(pc => pc.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

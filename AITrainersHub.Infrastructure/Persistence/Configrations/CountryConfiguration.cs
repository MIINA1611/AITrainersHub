using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.NameAr).IsRequired().HasMaxLength(100);
            builder.Property(c => c.NameEn).IsRequired().HasMaxLength(100);
            builder.Property(c => c.IsoCode).IsRequired().HasMaxLength(2);

            builder.HasIndex(c => c.IsoCode).IsUnique();
        }
    }
}

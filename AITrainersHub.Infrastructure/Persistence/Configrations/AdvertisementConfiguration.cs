using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.ToTable("Advertisements");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Subtitle).HasMaxLength(300);
            builder.Property(a => a.CtaText).IsRequired().HasMaxLength(50);
            builder.Property(a => a.CtaUrl).IsRequired().HasMaxLength(300);
            builder.Property(a => a.Position).IsRequired().HasMaxLength(50);

            builder.Property(a => a.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt).IsRequired();
        }
    }
}

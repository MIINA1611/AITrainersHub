using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.ToTable("Platforms");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.DescriptionAr)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(p => p.DescriptionEn)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(p => p.WebsiteUrl)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(p => p.ReferralLink)
                .HasMaxLength(300);

            // بيتخزن كنص ("Open", "Soon", "Closed") بدل رقم، عشان يبقى واضح لو بصيت على الداتابيز مباشرة
            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.HasIndex(p => p.Name);
        }
    }
}

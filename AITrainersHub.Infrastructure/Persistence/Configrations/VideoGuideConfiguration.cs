using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class VideoGuideConfiguration : IEntityTypeConfiguration<VideoGuide>
    {
        public void Configure(EntityTypeBuilder<VideoGuide> builder)
        {
            builder.ToTable("VideoGuides");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Title).IsRequired().HasMaxLength(200);
            builder.Property(v => v.Url).IsRequired().HasMaxLength(500);
            builder.Property(v => v.SourceType).IsRequired().HasMaxLength(50);
            builder.Property(v => v.AuthorOrChannel).HasMaxLength(150);

            builder.Property(v => v.IsApproved)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(v => v.CreatedAt).IsRequired();

            builder.HasOne(v => v.Platform)
                .WithMany(p => p.VideoGuides)
                .HasForeignKey(v => v.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

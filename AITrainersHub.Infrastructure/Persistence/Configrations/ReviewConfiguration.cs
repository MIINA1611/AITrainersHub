using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReviewerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.IsApproved)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.HasOne(r => r.Platform)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);

            // يمنع حفظ تقييم غلط زي 0 أو 8 من غير ما تتحقق منه في الكود كل مرة
            builder.HasCheckConstraint("CK_Review_Rating_Range", "[Rating] >= 1 AND [Rating] <= 5");
        }
    }
}

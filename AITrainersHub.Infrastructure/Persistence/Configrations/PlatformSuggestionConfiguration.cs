using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class PlatformSuggestionConfiguration : IEntityTypeConfiguration<PlatformSuggestion>
    {
        public void Configure(EntityTypeBuilder<PlatformSuggestion> builder)
        {
            builder.ToTable("PlatformSuggestions");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.PlatformName).IsRequired().HasMaxLength(150);
            builder.Property(s => s.Url).IsRequired().HasMaxLength(300);
            builder.Property(s => s.Description).IsRequired().HasMaxLength(1000);
            builder.Property(s => s.SubmitterName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.EmailOfUser).HasMaxLength(150);

            builder.Property(s => s.IsReviewed)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(s => s.CreatedAt).IsRequired();

            // اختياري - لو المستخدم ماحددش تصنيف، الاقتراح يفضل موجود من غيره
            builder.HasOne(s => s.Category)
                .WithMany()
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

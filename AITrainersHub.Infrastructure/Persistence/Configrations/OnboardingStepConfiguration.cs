using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class OnboardingStepConfiguration : IEntityTypeConfiguration<OnboardingStep>
    {
        public void Configure(EntityTypeBuilder<OnboardingStep> builder)
        {
            builder.ToTable("OnboardingSteps");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StepNumber).IsRequired();
            builder.Property(s => s.TitleAr).IsRequired().HasMaxLength(150);
            builder.Property(s => s.TitleEn).IsRequired().HasMaxLength(150);
            builder.Property(s => s.DescriptionAr).IsRequired().HasMaxLength(1000);
            builder.Property(s => s.DescriptionEn).IsRequired().HasMaxLength(1000);
            builder.Property(s => s.Tip).HasMaxLength(300);
            builder.Property(s => s.ScreenshotUrl).HasMaxLength(300);

            builder.HasOne(s => s.Platform)
                .WithMany(p => p.OnboardingSteps)
                .HasForeignKey(s => s.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

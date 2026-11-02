using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class WorkOpportunityConfiguration : IEntityTypeConfiguration<WorkOpportunity>
    {
        public void Configure(EntityTypeBuilder<WorkOpportunity> builder)
        {
            builder.ToTable("WorkOpportunities");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Title).IsRequired().HasMaxLength(150);
            builder.Property(o => o.DescriptionAr).IsRequired().HasMaxLength(2000);
            builder.Property(o => o.DescriptionEn).IsRequired().HasMaxLength(2000);
            builder.Property(o => o.WhatsAppLink).IsRequired().HasMaxLength(300);

            builder.Property(o => o.IsApproved).IsRequired().HasDefaultValue(false);
            builder.Property(o => o.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(o => o.CreatedAt).IsRequired();

            builder.HasOne(o => o.Leader)
                .WithMany()
                .HasForeignKey(o => o.LeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Platform)
                .WithMany()
                .HasForeignKey(o => o.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

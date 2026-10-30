using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class PlatformCategoryConfiguration : IEntityTypeConfiguration<PlatformCategory>
    {
        public void Configure(EntityTypeBuilder<PlatformCategory> builder)
        {
            builder.ToTable("PlatformCategories");
            builder.HasKey(pc => new { pc.PlatformId, pc.CategoryId });

            builder.HasOne(pc => pc.Platform)
                .WithMany(p => p.PlatformCategories)
                .HasForeignKey(pc => pc.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pc => pc.Category)
                .WithMany(c => c.PlatformCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    public class PlatformPaymentMethodConfiguration : IEntityTypeConfiguration<PlatformPaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PlatformPaymentMethod> builder)
        {
            builder.ToTable("PlatformPaymentMethods");
            builder.HasKey(ppm => new { ppm.PlatformId, ppm.PaymentMethodId });

            builder.HasOne(ppm => ppm.Platform)
                .WithMany(p => p.PlatformPaymentMethods)
                .HasForeignKey(ppm => ppm.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ppm => ppm.PaymentMethod)
                .WithMany(pm => pm.PlatformPaymentMethods)
                .HasForeignKey(ppm => ppm.PaymentMethodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

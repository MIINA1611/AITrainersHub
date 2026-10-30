using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITrainersHub.Infrastructure.Persistence.Configurations
{
    // مفيش ToTable هنا عمداً - الجدول اسمه AspNetUsers أصلاً من IdentityDbContext
    // ده بس بيضيف قيد على الحقل الإضافي اللي زودناه
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.DisplayName)
                .HasMaxLength(100);
        }
    }
}

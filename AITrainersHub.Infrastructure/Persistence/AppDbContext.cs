using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AITrainersHub.Application.Interfaces;
namespace AITrainersHub.Infrastructure.Persistence
{
    /// <summary>
    /// Db context for the AI Trainers Hub application, extending IdentityDbContext to include custom user and role entities.
    /// </summary>
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid> , IAITrainerHubDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<Country> Countries => Set<Country>();

        public DbSet<Platform> Platforms => Set<Platform>();
        public DbSet<PlatformCategory> PlatformCategories => Set<PlatformCategory>();
        public DbSet<PlatformPaymentMethod> PlatformPaymentMethods => Set<PlatformPaymentMethod>();
        public DbSet<PlatformCountry> PlatformCountries => Set<PlatformCountry>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<VideoGuide> VideoGuides => Set<VideoGuide>();
        public DbSet<Advertisement> Advertisements => Set<Advertisement>();
        public DbSet<PlatformSuggestion> PlatformSuggestions => Set<PlatformSuggestion>();
        public DbSet<OnboardingStep> OnboardingSteps => Set<OnboardingStep>();
        public DbSet<WorkOpportunity> WorkOpportunities => Set<WorkOpportunity>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
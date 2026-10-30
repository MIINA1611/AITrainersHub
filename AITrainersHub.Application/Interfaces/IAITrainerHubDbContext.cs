using AITrainersHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AITrainersHub.Application.Interfaces
{
    public interface IAITrainerHubDbContext
    {
        DbSet<Advertisement> Advertisements { get; }
        DbSet<Category> Categories { get; }
        DbSet<Country> Countries { get; }
        DbSet<OnboardingStep> OnboardingSteps { get; }
        DbSet<PaymentMethod> PaymentMethods { get; }
        DbSet<Platform> Platforms { get; }
        DbSet<PlatformCategory> PlatformCategories { get; }
        DbSet<PlatformCountry> PlatformCountries { get; }
        DbSet<PlatformPaymentMethod> PlatformPaymentMethods { get; }
        DbSet<PlatformSuggestion> PlatformSuggestions { get; }
        DbSet<Review> Reviews { get; }
        DbSet<VideoGuide> VideoGuides { get; }
        DbSet<WorkOpportunity> WorkOpportunities { get; }
        DbSet<ApplicationUser> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

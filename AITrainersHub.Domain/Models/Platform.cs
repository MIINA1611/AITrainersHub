using static AITrainersHub.Domain.Enums.PlatfromStatus;

namespace AITrainersHub.Domain.Models
{
    public class Platform
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string? ReferralLink { get; set; }

        public PlatformStatus Status { get; set; } = PlatformStatus.Open;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PlatformCategory> PlatformCategories { get; set; } = new List<PlatformCategory>();
        public ICollection<PlatformPaymentMethod> PlatformPaymentMethods { get; set; } = new List<PlatformPaymentMethod>();
        public ICollection<PlatformCountry> PlatformCountries { get; set; } = new List<PlatformCountry>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<VideoGuide> VideoGuides { get; set; } = new List<VideoGuide>();
        public ICollection<OnboardingStep> OnboardingSteps { get; set; } = new List<OnboardingStep>();


        public Platform()
        {
            
        }
        public Platform(
            string name,
            string slug,
            string descriptionAr,
            string descriptionEn,
            string websiteUrl,
            string? referralLink,
            PlatformStatus status)
        {
            Name = name;
            Slug = slug;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            WebsiteUrl = websiteUrl;
            ReferralLink = referralLink;
            Status = status;
        }
        public void Update(
            string? name,
            string? slug,
            string? descriptionAr,
            string? descriptionEn,
            string? websiteUrl,
            string? referralLink,
            PlatformStatus? status)
        {
            Name = name ?? Name;
            Slug = slug ?? Slug;
            DescriptionAr = descriptionAr ?? DescriptionAr;
            DescriptionEn = descriptionEn ?? DescriptionEn;
            WebsiteUrl = websiteUrl ?? WebsiteUrl;
            ReferralLink = referralLink ?? ReferralLink;
            Status = status ?? Status;
        }
    }
}

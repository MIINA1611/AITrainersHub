using System.Linq.Expressions;
using AITrainersHub.Application.Features.OnboardingStep.DTOs;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.PlatformCategory.DTOs;
using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Features.PlatformPaymentMethod.DTOs;
using AITrainersHub.Application.Features.Review.DTOs;
using AITrainersHub.Application.Features.VideoGuide.DTOs;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.Platform.Handlers.QueriesHandlers
{
    /// <summary>
    /// Shared projection used by the platform details queries.
    /// </summary>
    internal static class PlatformDetailsProjection
    {
        public static readonly Expression<Func<PlatformEntity, PlatformDetailsDto>> Details =
            p => new PlatformDetailsDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                DescriptionAr = p.DescriptionAr,
                DescriptionEn = p.DescriptionEn,
                WebsiteUrl = p.WebsiteUrl,
                ReferralLink = p.ReferralLink,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                Categories = p.PlatformCategories
                    .OrderBy(pc => pc.Category.Name)
                    .Select(pc => new PlatformCategoryDto
                    {
                        CategoryId = pc.CategoryId,
                        Name = pc.Category.Name,
                        Description = pc.Category.Description
                    })
                    .ToList(),
                PaymentMethods = p.PlatformPaymentMethods
                    .OrderBy(ppm => ppm.PaymentMethod.Name)
                    .Select(ppm => new PlatformPaymentMethodDto
                    {
                        PaymentMethodId = ppm.PaymentMethodId,
                        Name = ppm.PaymentMethod.Name
                    })
                    .ToList(),
                Countries = p.PlatformCountries
                    .OrderBy(pc => pc.Country.NameEn)
                    .Select(pc => new PlatformCountryDto
                    {
                        CountryId = pc.CountryId,
                        NameAr = pc.Country.NameAr,
                        NameEn = pc.Country.NameEn,
                        IsoCode = pc.Country.IsoCode,
                        IsFullySupported = pc.IsFullySupported,
                        Notes = pc.Notes
                    })
                    .ToList(),
                Reviews = p.Reviews
                    .Where(r => r.IsApproved)
                    .OrderByDescending(r => r.CreatedAt)
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        PlatformId = r.PlatformId,
                        ReviewerName = r.ReviewerName,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        IsApproved = r.IsApproved,
                        CreatedAt = r.CreatedAt
                    })
                    .ToList(),
                VideoGuides = p.VideoGuides
                    .Where(v => v.IsApproved)
                    .OrderByDescending(v => v.CreatedAt)
                    .Select(v => new VideoGuideDto
                    {
                        Id = v.Id,
                        PlatformId = v.PlatformId,
                        Title = v.Title,
                        Language = v.Language,
                        Url = v.Url,
                        SourceType = v.SourceType,
                        AuthorOrChannel = v.AuthorOrChannel,
                        PublishedAt = v.PublishedAt,
                        IsApproved = v.IsApproved,
                        CreatedAt = v.CreatedAt
                    })
                    .ToList(),
                OnboardingSteps = p.OnboardingSteps
                    .OrderBy(s => s.StepNumber)
                    .Select(s => new OnboardingStepDto
                    {
                        Id = s.Id,
                        PlatformId = s.PlatformId,
                        StepNumber = s.StepNumber,
                        TitleAr = s.TitleAr,
                        TitleEn = s.TitleEn,
                        DescriptionAr = s.DescriptionAr,
                        DescriptionEn = s.DescriptionEn,
                        Tip = s.Tip,
                        ScreenshotUrl = s.ScreenshotUrl
                    })
                    .ToList()
            };
    }
}

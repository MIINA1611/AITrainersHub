using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.PlatformCategory.DTOs;
using AITrainersHub.Application.Features.PlatformCountry.DTOs;
using AITrainersHub.Application.Features.PlatformPaymentMethod.DTOs;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformEntity = AITrainersHub.Domain.Models.Platform;

namespace AITrainersHub.Application.Features.Platform.Handlers.QueriesHandlers
{
    /// <summary>
    /// Handles the ReadPlatformsQuery and applies the optional filters.
    /// </summary>
    public class ReadPlatformsQueryHandler(IAITrainerHubDbContext context)
        : IRequestHandler<ReadPlatformsQuery, List<PlatformListResponse>>
    {
        public async Task<List<PlatformListResponse>> Handle(
            ReadPlatformsQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<PlatformEntity> query = context.Platforms.AsNoTracking();

            if (request.Status.HasValue)
            {
                query = query.Where(p => p.Status == request.Status.Value);
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(p => p.PlatformCategories.Any(pc => pc.CategoryId == request.CategoryId.Value));
            }

            if (request.CountryId.HasValue)
            {
                query = query.Where(p => p.PlatformCountries.Any(pc => pc.CountryId == request.CountryId.Value));
            }

            if (request.PaymentMethodId.HasValue)
            {
                query = query.Where(p => p.PlatformPaymentMethods.Any(ppm => ppm.PaymentMethodId == request.PaymentMethodId.Value));
            }

            return await query
                .OrderBy(p => p.Name)
                .Select(p => new PlatformListResponse
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
                    ReviewCount = p.Reviews.Count(r => r.IsApproved),
                    AverageRating = p.Reviews.Any(r => r.IsApproved)
                        ? p.Reviews.Where(r => r.IsApproved).Average(r => (double)r.Rating)
                        : 0
                })
                .ToListAsync(cancellationToken);
        }
    }
}

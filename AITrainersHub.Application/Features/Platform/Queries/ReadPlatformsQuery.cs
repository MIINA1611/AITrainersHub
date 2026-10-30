using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainersHub.Application.Features.Platform.Queries
{
    /// <summary>
    /// Represents a query to read the platforms list with optional filters.
    /// </summary>
    public class ReadPlatformsQuery : IRequest<List<PlatformListResponse>>, ICacheableQuery
    {
        public PlatformStatus? Status { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? PaymentMethodId { get; set; }

        public string CacheKey
        {
            get
            {
                if (!Status.HasValue && !CategoryId.HasValue && !CountryId.HasValue && !PaymentMethodId.HasValue)
                {
                    return nameof(ReadPlatformsQuery);
                }

                return $"{nameof(ReadPlatformsQuery)}_{Status?.ToString() ?? "all"}_{CategoryId?.ToString() ?? "all"}_{CountryId?.ToString() ?? "all"}_{PaymentMethodId?.ToString() ?? "all"}";
            }
        }

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadPlatformsQuery()
        {
        }

        public ReadPlatformsQuery(
            PlatformStatus? status = null,
            Guid? categoryId = null,
            Guid? countryId = null,
            Guid? paymentMethodId = null)
        {
            Status = status;
            CategoryId = categoryId;
            CountryId = countryId;
            PaymentMethodId = paymentMethodId;
        }
    }
}

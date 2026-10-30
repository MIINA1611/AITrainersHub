using AITrainersHub.Application.Features.PlatformPaymentMethod.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformPaymentMethod.Queries
{
    /// <summary>
    /// Represents a query to read all payment methods linked to a platform.
    /// </summary>
    public class ReadPaymentMethodsByPlatformIdQuery : IRequest<List<PlatformPaymentMethodDto>>, ICacheableQuery
    {
        public Guid PlatformId { get; set; }

        public string CacheKey => $"{nameof(ReadPaymentMethodsByPlatformIdQuery)}_{PlatformId}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadPaymentMethodsByPlatformIdQuery()
        {
        }

        public ReadPaymentMethodsByPlatformIdQuery(Guid platformId)
        {
            PlatformId = platformId;
        }
    }
}

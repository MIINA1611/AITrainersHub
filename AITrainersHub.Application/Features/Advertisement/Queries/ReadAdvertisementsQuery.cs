using AITrainersHub.Application.Features.Advertisement.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Advertisement.Queries
{
    /// <summary>
    /// Represents a query to read advertisements, optionally filtered by active state.
    /// </summary>
    public class ReadAdvertisementsQuery : IRequest<List<AdvertisementDto>>, ICacheableQuery
    {
        public bool? IsActive { get; set; }

        public string CacheKey =>
            !IsActive.HasValue
                ? nameof(ReadAdvertisementsQuery)
                : $"{nameof(ReadAdvertisementsQuery)}_{(IsActive.Value ? "active" : "inactive")}";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadAdvertisementsQuery()
        {
        }

        public ReadAdvertisementsQuery(bool? isActive = null)
        {
            IsActive = isActive;
        }
    }
}

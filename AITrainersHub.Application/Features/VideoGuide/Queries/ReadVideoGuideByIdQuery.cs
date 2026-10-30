using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.VideoGuide.Queries
{
    /// <summary>
    /// Represents a query to read a video guide by its unique identifier.
    /// </summary>
    public class ReadVideoGuideByIdQuery : IRequest<VideoGuideDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"{nameof(ReadVideoGuideByIdQuery)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadVideoGuideByIdQuery()
        {
        }

        public ReadVideoGuideByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

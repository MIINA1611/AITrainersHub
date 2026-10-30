using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.WorkOpportunity.Queries
{
    /// <summary>
    /// Represents a query to read a work opportunity by its unique identifier.
    /// </summary>
    public class ReadWorkOpportunityByIdQuery : IRequest<WorkOpportunityDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"{nameof(ReadWorkOpportunityByIdQuery)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadWorkOpportunityByIdQuery()
        {
        }

        public ReadWorkOpportunityByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

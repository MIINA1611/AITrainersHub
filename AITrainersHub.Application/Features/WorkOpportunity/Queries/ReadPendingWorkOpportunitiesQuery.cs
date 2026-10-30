using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.WorkOpportunity.Queries
{
    /// <summary>
    /// Represents a query to read all work opportunities awaiting approval.
    /// </summary>
    public class ReadPendingWorkOpportunitiesQuery : IRequest<List<WorkOpportunityDto>>, ICacheableQuery
    {
        public string CacheKey => nameof(ReadPendingWorkOpportunitiesQuery);
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}

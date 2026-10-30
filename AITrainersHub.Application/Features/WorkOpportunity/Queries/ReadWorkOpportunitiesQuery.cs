using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.WorkOpportunity.Queries
{
    /// <summary>
    /// Represents a query to read work opportunities with optional filters.
    /// </summary>
    public class ReadWorkOpportunitiesQuery : IRequest<List<WorkOpportunityDto>>, ICacheableQuery
    {
        public Guid? LeaderId { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsActive { get; set; }

        public string CacheKey
        {
            get
            {
                if (!LeaderId.HasValue && !IsApproved.HasValue && !IsActive.HasValue)
                {
                    return nameof(ReadWorkOpportunitiesQuery);
                }

                return $"{nameof(ReadWorkOpportunitiesQuery)}_{LeaderId?.ToString() ?? "all"}_{IsApproved?.ToString() ?? "all"}_{IsActive?.ToString() ?? "all"}";
            }
        }

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadWorkOpportunitiesQuery()
        {
        }

        public ReadWorkOpportunitiesQuery(Guid? leaderId, bool? isApproved = null, bool? isActive = null)
        {
            LeaderId = leaderId;
            IsApproved = isApproved;
            IsActive = isActive;
        }
    }
}

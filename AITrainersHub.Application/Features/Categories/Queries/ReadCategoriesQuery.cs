using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Models;
using MediatR;

namespace AITrainersHub.Application.Features.Categories.Queries
{
    public class ReadCategoriesQuery : IRequest<List<CategoryListResponse>> , ICacheableQuery
    {
        public string CacheKey => nameof(ReadCategoriesQuery);
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}

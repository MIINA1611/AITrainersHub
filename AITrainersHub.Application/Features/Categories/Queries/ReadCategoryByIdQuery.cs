using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Models;
using MediatR;

namespace AITrainersHub.Application.Features.Categories.Queries
{
    /// <summary>
    /// Represents a query to read a category by its unique identifier (Id).
    /// </summary>
    public class ReadCategoryById : IRequest<CategoryDto>, ICacheableQuery
    {
        public Guid Id { get; set; }
        public string CacheKey => $"{nameof(ReadCategoryById)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadCategoryById()
        {
        }

        public ReadCategoryById(Guid id)
        {
            Id = id;
        }
    }
}

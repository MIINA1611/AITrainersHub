using AITrainersHub.Application.Features.PlatformCategory.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformCategory.Commands
{
    /// <summary>
    /// Represents a command to link a category to a platform.
    /// </summary>
    public class AddPlatformCategoryCommand : IRequest<PlatformCategoryDto>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public Guid CategoryId { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddPlatformCategoryCommand()
        {
        }

        public AddPlatformCategoryCommand(Guid platformId, Guid categoryId)
        {
            PlatformId = platformId;
            CategoryId = categoryId;
        }
    }
}

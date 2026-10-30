using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PlatformCategory.Commands
{
    /// <summary>
    /// Represents a command to unlink a category from a platform.
    /// </summary>
    public class DeletePlatformCategoryCommand : IRequest<bool>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public Guid CategoryId { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public DeletePlatformCategoryCommand()
        {
        }

        public DeletePlatformCategoryCommand(Guid platformId, Guid categoryId)
        {
            PlatformId = platformId;
            CategoryId = categoryId;
        }
    }
}

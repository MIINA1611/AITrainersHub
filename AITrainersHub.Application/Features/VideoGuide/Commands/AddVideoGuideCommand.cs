using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Enums;
using MediatR;

namespace AITrainersHub.Application.Features.VideoGuide.Commands
{
    /// <summary>
    /// Represents a command to add a new video guide to a platform.
    /// </summary>
    public class AddVideoGuideCommand : IRequest<VideoGuideDto>, ICacheInvalidatorCommand
    {
        public Guid PlatformId { get; set; }
        public string Title { get; set; } = string.Empty;
        public GuideLanguage Language { get; set; } = GuideLanguage.Arabic;
        public string Url { get; set; } = string.Empty;
        public string SourceType { get; set; } = "YouTube";
        public string? AuthorOrChannel { get; set; }
        public DateTime? PublishedAt { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddVideoGuideCommand()
        {
        }

        public AddVideoGuideCommand(
            Guid platformId,
            string title,
            GuideLanguage language,
            string url,
            string sourceType,
            string? authorOrChannel,
            DateTime? publishedAt)
        {
            PlatformId = platformId;
            Title = title;
            Language = language;
            Url = url;
            SourceType = sourceType;
            AuthorOrChannel = authorOrChannel;
            PublishedAt = publishedAt;
        }
    }
}

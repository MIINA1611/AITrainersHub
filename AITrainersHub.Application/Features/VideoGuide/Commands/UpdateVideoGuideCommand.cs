using AITrainersHub.Application.Features.VideoGuide.DTOs;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Domain.Enums;
using MediatR;

namespace AITrainersHub.Application.Features.VideoGuide.Commands
{
    /// <summary>
    /// Represents a command to update an existing video guide, including its approval state.
    /// </summary>
    public class UpdateVideoGuideCommand : IRequest<VideoGuideDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public GuideLanguage? Language { get; set; }
        public string? Url { get; set; }
        public string? SourceType { get; set; }
        public string? AuthorOrChannel { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool? IsApproved { get; set; }

        public HashSet<string> CacheKeySet { get; } = new();
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdateVideoGuideCommand()
        {
        }

        public UpdateVideoGuideCommand(
            Guid id,
            string? title,
            GuideLanguage? language,
            string? url,
            string? sourceType,
            string? authorOrChannel,
            DateTime? publishedAt,
            bool? isApproved)
        {
            Id = id;
            Title = title;
            Language = language;
            Url = url;
            SourceType = sourceType;
            AuthorOrChannel = authorOrChannel;
            PublishedAt = publishedAt;
            IsApproved = isApproved;
            CacheKeySet.Add($"{nameof(ReadVideoGuideByIdQuery)}_{id}");
        }
    }
}

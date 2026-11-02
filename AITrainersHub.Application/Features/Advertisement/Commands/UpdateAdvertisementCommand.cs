using AITrainersHub.Application.Features.Advertisement.DTOs;
using AITrainersHub.Application.Features.Advertisement.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Advertisement.Commands
{
    /// <summary>
    /// Represents a command to update an existing advertisement.
    /// </summary>
    public class UpdateAdvertisementCommand : IRequest<AdvertisementDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? CtaText { get; set; }
        public string? CtaUrl { get; set; }
        public string? Position { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// When true, the supplied StartDate and EndDate replace the stored values,
        /// including clearing them when both are null. Other callers (for example
        /// the active toggle) leave this false so a partial update never wipes dates.
        /// </summary>
        public bool UpdateSchedule { get; set; }

        public HashSet<string> CacheKeySet { get; } = new()
        {
            nameof(ReadAdvertisementsQuery),
            $"{nameof(ReadAdvertisementsQuery)}_active",
            $"{nameof(ReadAdvertisementsQuery)}_inactive"
        };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdateAdvertisementCommand()
        {
        }

        public UpdateAdvertisementCommand(
            Guid id,
            string? title,
            string? subtitle,
            string? ctaText,
            string? ctaUrl,
            string? position,
            bool? isActive,
            DateTime? startDate,
            DateTime? endDate)
        {
            Id = id;
            Title = title;
            Subtitle = subtitle;
            CtaText = ctaText;
            CtaUrl = ctaUrl;
            Position = position;
            IsActive = isActive;
            StartDate = startDate;
            EndDate = endDate;
            CacheKeySet.Add($"{nameof(ReadAdvertisementByIdQuery)}_{id}");
        }
    }
}

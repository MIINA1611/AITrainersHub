using AITrainersHub.Application.Features.Advertisement.DTOs;
using AITrainersHub.Application.Features.Advertisement.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.Advertisement.Commands
{
    /// <summary>
    /// Represents a command to create a new advertisement.
    /// </summary>
    public class AddAdvertisementCommand : IRequest<AdvertisementDto>, ICacheInvalidatorCommand
    {
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string CtaText { get; set; } = "Book now";
        public string CtaUrl { get; set; } = string.Empty;
        public string Position { get; set; } = "Hero";
        public bool IsActive { get; set; } = true;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public HashSet<string> CacheKeySet { get; } = new()
        {
            nameof(ReadAdvertisementsQuery),
            $"{nameof(ReadAdvertisementsQuery)}_active",
            $"{nameof(ReadAdvertisementsQuery)}_inactive"
        };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddAdvertisementCommand()
        {
        }

        public AddAdvertisementCommand(
            string title,
            string? subtitle,
            string ctaText,
            string ctaUrl,
            string position,
            bool isActive,
            DateTime? startDate,
            DateTime? endDate)
        {
            Title = title;
            Subtitle = subtitle;
            CtaText = ctaText;
            CtaUrl = ctaUrl;
            Position = position;
            IsActive = isActive;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

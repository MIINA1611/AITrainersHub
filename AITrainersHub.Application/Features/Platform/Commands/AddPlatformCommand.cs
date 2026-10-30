using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainersHub.Application.Features.Platform.Commands
{
    /// <summary>
    /// Represents a command to create a new platform.
    /// </summary>
    public class AddPlatformCommand : IRequest<PlatformDto>, ICacheInvalidatorCommand
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string? ReferralLink { get; set; }
        public PlatformStatus Status { get; set; } = PlatformStatus.Open;

        public HashSet<string> CacheKeySet { get; } = new() { nameof(ReadPlatformsQuery) };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public AddPlatformCommand()
        {
        }

        public AddPlatformCommand(
            string name,
            string slug,
            string descriptionAr,
            string descriptionEn,
            string websiteUrl,
            string? referralLink,
            PlatformStatus status)
        {
            Name = name;
            Slug = slug;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            WebsiteUrl = websiteUrl;
            ReferralLink = referralLink;
            Status = status;
        }
    }
}

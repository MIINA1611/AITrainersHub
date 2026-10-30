using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Interfaces;
using MediatR;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainersHub.Application.Features.Platform.Commands
{
    /// <summary>
    /// Represents a command to update an existing platform. Only supplied values are applied.
    /// </summary>
    public class UpdatePlatformCommand : IRequest<PlatformDto>, ICacheInvalidatorCommand
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? ReferralLink { get; set; }
        public PlatformStatus? Status { get; set; }

        public HashSet<string> CacheKeySet { get; } = new() { nameof(ReadPlatformsQuery) };
        public IEnumerable<string> CacheKeys => CacheKeySet;

        public UpdatePlatformCommand()
        {
        }

        public UpdatePlatformCommand(
            Guid id,
            string? name,
            string? slug,
            string? descriptionAr,
            string? descriptionEn,
            string? websiteUrl,
            string? referralLink,
            PlatformStatus? status)
        {
            Id = id;
            Name = name;
            Slug = slug;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            WebsiteUrl = websiteUrl;
            ReferralLink = referralLink;
            Status = status;
            CacheKeySet.Add($"{nameof(ReadPlatformByIdQuery)}_{id}");
        }
    }
}

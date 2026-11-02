using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.Platform.DTOs;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data displayed on the public platform directory.
    /// </summary>
    public class PublicPlatformsIndexViewModel
    {
        public List<PlatformListResponse> Platforms { get; set; } = new();
        public List<CategoryListResponse> Categories { get; set; } = new();
        public List<CountryDto> Countries { get; set; } = new();
        public List<PaymentMethodDto> PaymentMethods { get; set; } = new();

        public string? Search { get; set; }
        public PlatformStatus? Status { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? PaymentMethodId { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalCount { get; set; }
        public int TotalPages { get; set; } = 1;

        public bool HasFilters =>
            !string.IsNullOrWhiteSpace(Search) ||
            Status.HasValue ||
            CategoryId.HasValue ||
            CountryId.HasValue ||
            PaymentMethodId.HasValue;
    }
}

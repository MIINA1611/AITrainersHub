using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.Platform.DTOs;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Shared data for the public platform directory used by the home page
    /// and the dedicated platforms listing page.
    /// </summary>
    public class PlatformDirectoryViewModel
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

    /// <summary>
    /// Wraps the directory with the route details needed by the shared partial.
    /// </summary>
    public class PlatformDirectorySectionViewModel
    {
        public PlatformDirectoryViewModel Directory { get; set; } = new();
        public string Area { get; set; } = "Users";
        public string Controller { get; set; } = "Platforms";
        public string Action { get; set; } = "Index";
        public string? Fragment { get; set; }
    }
}

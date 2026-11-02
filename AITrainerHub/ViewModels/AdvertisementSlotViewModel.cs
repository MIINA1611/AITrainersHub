using AITrainersHub.Application.Features.Advertisement.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// One advertisement slot rendered by the AdvertisementSlot view component.
    /// </summary>
    public class AdvertisementSlotViewModel
    {
        public string Position { get; set; } = string.Empty;
        public AdvertisementDto? Advertisement { get; set; }
    }

    /// <summary>
    /// Shared pagination data for public list pages.
    /// </summary>
    public class PaginationViewModel
    {
        public string Area { get; set; } = "Users";
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = "Index";
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 9;
        public Dictionary<string, string> RouteValues { get; set; } = new();

        public int FirstItem => TotalCount == 0 ? 0 : ((Page - 1) * PageSize) + 1;
        public int LastItem => Math.Min(Page * PageSize, TotalCount);
    }
}

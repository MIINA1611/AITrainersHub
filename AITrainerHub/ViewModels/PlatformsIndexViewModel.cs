using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data required by the platforms list page.
    /// </summary>
    public class PlatformsIndexViewModel
    {
        public List<PlatformRowViewModel> Platforms { get; set; } = new();

        public string? Search { get; set; }
        public PlatformStatus? Status { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public int TotalPages => TotalCount == 0
            ? 1
            : (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;

        public int FirstItem => TotalCount == 0 ? 0 : ((Page - 1) * PageSize) + 1;
        public int LastItem => Math.Min(Page * PageSize, TotalCount);
    }

    /// <summary>
    /// One row in the platforms table.
    /// </summary>
    public class PlatformRowViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public PlatformStatus Status { get; set; }
        public string Categories { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

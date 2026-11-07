using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Platform.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data displayed on the public home page.
    /// </summary>
    public class PublicHomeViewModel
    {
        public List<PlatformListResponse> FeaturedPlatforms { get; set; } = new();
        public List<CategoryListResponse> Categories { get; set; } = new();
        public int TotalPlatforms { get; set; }
    }
}

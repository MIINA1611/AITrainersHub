using AITrainerHub.Request_Models.Public;
using AITrainersHub.Application.Features.Categories.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Backing model for the public suggestion form.
    /// </summary>
    public class SuggestPlatformViewModel
    {
        public PlatformSuggestionRequest Form { get; set; } = new();
        public List<CategoryListResponse> Categories { get; set; } = new();
    }
}

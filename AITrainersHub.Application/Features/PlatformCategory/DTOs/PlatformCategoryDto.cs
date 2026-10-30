namespace AITrainersHub.Application.Features.PlatformCategory.DTOs
{
    /// <summary>
    /// Represents a category linked to a platform.
    /// </summary>
    public class PlatformCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}

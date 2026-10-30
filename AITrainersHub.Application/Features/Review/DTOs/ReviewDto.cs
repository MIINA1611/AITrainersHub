namespace AITrainersHub.Application.Features.Review.DTOs
{
    /// <summary>
    /// Represents a platform review.
    /// </summary>
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid PlatformId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

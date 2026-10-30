using AITrainersHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
     public class VideoGuide
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public GuideLanguage Language { get; set; } = GuideLanguage.Arabic;
        public string Url { get; set; } = string.Empty;
        public string SourceType { get; set; } = "YouTube"; // YouTube / Article
        public string? AuthorOrChannel { get; set; }
        public DateTime? PublishedAt { get; set; }

        // يظهر للزوار بعد موافقتك من الـ Admin
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public VideoGuide()
        {
            
        }
        public VideoGuide(
            Guid platformId,
            string title,
            GuideLanguage language,
            string url,
            string sourceType,
            string? authorOrChannel,
            DateTime? publishedAt)
        {
            PlatformId = platformId;
            Title = title;
            Language = language;
            Url = url;
            SourceType = sourceType;
            AuthorOrChannel = authorOrChannel;
            PublishedAt = publishedAt;
        }
        public void Update(
            string? title,
            GuideLanguage? language,
            string? url,
            string? sourceType,
            string? authorOrChannel,
            DateTime? publishedAt,
            bool? isApproved)
        {
            Title = title ?? Title;
            Language = language ?? Language;
            Url = url ?? Url;
            SourceType = sourceType ?? SourceType;
            AuthorOrChannel = authorOrChannel ?? AuthorOrChannel;
            PublishedAt = publishedAt ?? PublishedAt;
            IsApproved = isApproved ?? IsApproved;
        }
    }
}

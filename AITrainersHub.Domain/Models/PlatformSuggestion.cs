using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class PlatformSuggestion
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string PlatformName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SubmitterName { get; set; } = string.Empty;
        public string? EmailOfUser { get; set; } // optional legacy contact field

        // اختياري - المستخدم مش لازم يعرف التصنيف بالظبط
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        public bool IsReviewed { get; set; } = false; // بتتحول لـ Platform حقيقي بعد موافقتك يدوياً
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public PlatformSuggestion()
        {
            
        }
        public PlatformSuggestion(
            string platformName,
            string url,
            string description,
            string submitterName,
            Guid? categoryId)
        {
            PlatformName = platformName;
            Url = url;
            Description = description;
            SubmitterName = submitterName;
            CategoryId = categoryId;
        }
        public void Update(
            string? platformName,
            string? url,
            string? description,
            string? submitterName,
            string? emailOfUser,
            Guid? categoryId,
            bool? isReviewed)
        {
            PlatformName = platformName ?? PlatformName;
            Url = url ?? Url;
            Description = description ?? Description;
            SubmitterName = submitterName ?? SubmitterName;
            EmailOfUser = emailOfUser ?? EmailOfUser;
            CategoryId = categoryId ?? CategoryId;
            IsReviewed = isReviewed ?? IsReviewed;
        }
    }
}

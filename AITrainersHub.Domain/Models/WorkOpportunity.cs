using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class WorkOpportunity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid LeaderId { get; set; }
        public ApplicationUser Leader { get; set; } = null!;

        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string WhatsAppLink { get; set; } = string.Empty;

        public bool IsApproved { get; set; } = false; // موافقتك قبل ما تظهر
        public bool IsActive { get; set; } = true;     // الليدر يقفلها بنفسه لو المكان امتلا
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public WorkOpportunity()
        {
            
        }
        public WorkOpportunity(
            Guid leaderId,
            Guid platformId,
            string title,
            string descriptionAr,
            string descriptionEn,
            string whatsAppLink)
        {
            LeaderId = leaderId;
            PlatformId = platformId;
            Title = title;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            WhatsAppLink = whatsAppLink;
        }
        public void Update(
            Guid? platformId,
            string? title,
            string? descriptionAr,
            string? descriptionEn,
            string? whatsAppLink,
            bool? isApproved,
            bool? isActive)
        {
            PlatformId = platformId ?? PlatformId;
            Title = title ?? Title;
            DescriptionAr = descriptionAr ?? DescriptionAr;
            DescriptionEn = descriptionEn ?? DescriptionEn;
            WhatsAppLink = whatsAppLink ?? WhatsAppLink;
            IsApproved = isApproved ?? IsApproved;
            IsActive = isActive ?? IsActive;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class Advertisement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string CtaText { get; set; } = "Book now";
        public string CtaUrl { get; set; } = string.Empty;

        // مكان ظهور البانر: Hero, Sidebar, BetweenCards ... نص بسيط دلوقتي مش enum
        public string Position { get; set; } = "Hero";

        public bool IsActive { get; set; } = true;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public Advertisement()
        {
            
        }
        public Advertisement(
            string title,
            string? subtitle,
            string ctaText,
            string ctaUrl,
            string position,
            bool isActive,
            DateTime? startDate,
            DateTime? endDate)
        {
            Title = title;
            Subtitle = subtitle;
            CtaText = ctaText;
            CtaUrl = ctaUrl;
            Position = position;
            IsActive = isActive;
            StartDate = startDate;
            EndDate = endDate;
        }
        public void Update(
            string? title,
            string? subtitle,
            string? ctaText,
            string? ctaUrl,
            string? position,
            bool? isActive,
            DateTime? startDate,
            DateTime? endDate)
        {
            Title = title ?? Title;
            Subtitle = subtitle ?? Subtitle;
            CtaText = ctaText ?? CtaText;
            CtaUrl = ctaUrl ?? CtaUrl;
            Position = position ?? Position;
            IsActive = isActive ?? IsActive;
            StartDate = startDate ?? StartDate;
            EndDate = endDate ?? EndDate;
        }
    }
}

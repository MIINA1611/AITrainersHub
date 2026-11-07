using System.Globalization;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.Presentation
{
    /// <summary>
    /// Small presentation helpers shared by the public and Leader Razor views.
    /// </summary>
    public static class DisplayHelpers
    {
        public static string Initials(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "AI";
            }

            var parts = name
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Take(2)
                .Select(part => char.ToUpperInvariant(part[0]))
                .ToArray();

            return parts.Length == 0 ? "AI" : new string(parts);
        }

        public static string PlatformStatusCss(PlatformStatus status) => status switch
        {
            PlatformStatus.Open => "admin-badge--green",
            PlatformStatus.Soon => "admin-badge--amber",
            _ => "admin-badge--red"
        };

        public static string OpportunityStatus(WorkOpportunityDto opportunity)
        {
            if (!opportunity.IsApproved)
            {
                return "Pending";
            }

            return opportunity.IsActive ? "Live" : "Hidden";
        }

        public static string OpportunityStatusCss(WorkOpportunityDto opportunity)
        {
            if (!opportunity.IsApproved)
            {
                return "admin-badge--amber";
            }

            return opportunity.IsActive ? "admin-badge--green" : "admin-badge--gray";
        }

        public static string OpportunityStatusTitle(WorkOpportunityDto opportunity)
        {
            if (!opportunity.IsApproved)
            {
                return "Waiting for administrator approval.";
            }

            return opportunity.IsActive
                ? "Visible to the public."
                : "Approved but hidden from the public board.";
        }

        public static string PreferredDescription(string? arabic, string? english)
        {
            if (!string.IsNullOrWhiteSpace(english))
            {
                return english.Trim();
            }

            return string.IsNullOrWhiteSpace(arabic) ? string.Empty : arabic.Trim();
        }

        public static string Excerpt(string? value, int maxLength = 150)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalized = string.Join(
                " ",
                value.Split(
                    new[] { ' ', '\r', '\n', '\t' },
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

            if (normalized.Length <= maxLength)
            {
                return normalized;
            }

            return normalized[..maxLength].TrimEnd() + "...";
        }

        public static string Rating(double rating) =>
            rating.ToString("0.0", CultureInfo.InvariantCulture);

        public static string Date(DateTime value) =>
            value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}

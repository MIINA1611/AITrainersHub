namespace AITrainerHub.Presentation
{
    /// <summary>
    /// Centralizes the public brand name and the document title format used by
    /// every layout in the application.
    /// </summary>
    public static class Branding
    {
        public const string Name = "AI Trainers Hub";

        public const string HomeTitle =
            "AI Trainers Hub — Verified Directory & Opportunities for AI Trainers";

        /// <summary>
        /// Builds "Page Title | AI Trainers Hub" while allowing the homepage to
        /// use its own complete title without a duplicated suffix.
        /// </summary>
        public static string DocumentTitle(string? pageTitle)
        {
            if (string.IsNullOrWhiteSpace(pageTitle))
            {
                return Name;
            }

            var title = pageTitle.Trim();

            if (title.Contains(Name, StringComparison.OrdinalIgnoreCase))
            {
                return title;
            }

            return $"{title} | {Name}";
        }
    }
}

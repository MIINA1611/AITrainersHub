using Microsoft.AspNetCore.Identity.UI.Services;

namespace AITrainerHub.Services
{
    /// <summary>
    /// Minimal email sender used until a real email provider is configured.
    /// Keeps the scaffolded Identity pages constructible without changing
    /// their page model bindings or authentication flow.
    /// </summary>
    public sealed class NoOpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}

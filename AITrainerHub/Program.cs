using AITrainersHub.Infrastructure;
using AITrainerHub.Services;
using huzcodes.Extensions.Exceptions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;
using AITrainersHub.Application;
using FluentValidation;

namespace AITrainerHub
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.AddDbContext();
            builder.AddIdentity();

            // All protected routes, including the whole Admin Area, challenge
            // anonymous visitors with the real Identity sign-in page. Accounts
            // that are signed in without the required role land on AccessDenied.
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.ReturnUrlParameter = "returnUrl";
                options.Cookie.Name = "AITrainersHub.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;
            });

            builder.Services.AddSingleton<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, NoOpEmailSender>();
            builder.AddLimiter();
            builder.AddCacheService();        
            builder.Services.AddApplicationRegistration();
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            var app = builder.Build();

            await app.Services.InitializeDatabaseAsync();  
            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Users/Home/Error");
                app.UseHsts();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.AddExceptionHandlerExtension();
            app.UseHttpsRedirection();

            // Serve physical wwwroot files directly. MapStaticAssets' gzip endpoints
            // currently return an empty body for the admin CSS/JS in this environment.
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.MapStaticAssets();

            app.MapRazorPages();

            // The Admin Area has no Home controller; its entry point is the
            // dashboard. Mapping it explicitly makes /Admin resolve to
            // Dashboard/Index, so the inherited authorization redirects
            // anonymous visitors to the sign-in page instead of returning 404.
            app.MapAreaControllerRoute(
                name: "admin",
                areaName: "Admin",
                pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { area = "Users" })
                .WithStaticAssets();

            await app.RunAsync();
        }
    }
}

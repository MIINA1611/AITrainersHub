using AITrainersHub.Infrastructure;
using AITrainerHub.Services;
using huzcodes.Extensions.Exceptions;
using Microsoft.AspNetCore.HttpOverrides;
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

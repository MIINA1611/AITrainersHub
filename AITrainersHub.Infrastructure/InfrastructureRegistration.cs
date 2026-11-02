using AITrainersHub.Domain.Models;
using AITrainersHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using huzcodes.Persistence.Interfaces.Repositories;
using AITrainersHub.Application.Interfaces;
using AITrainersHub.Infrastructure.Implementation;

namespace AITrainersHub.Infrastructure
{
    public static class InfrastructureRegistration
    {
        /// <summary>
        /// Adds the application's DbContext to the service collection with SQL Server configuration.
        /// </summary>
        public static void AddDbContext(this WebApplicationBuilder builder)
        {
            // 1) قاعدة البيانات
            // بافتراض SQL Server واسم الـ connection string "DefaultConnection" في appsettings.json - غيّرهم لو مختلفين عندك
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AITrainerHubDB")));

            builder.Services.AddScoped<IAITrainerHubDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        }

        /// <summary>
        /// Adds ASP.NET Core Identity services to the service collection with custom password and sign-in options.
        /// </summary>
        public static void AddIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
            {
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequiredLength = 8;
                option.SignIn.RequireConfirmedEmail = false;
                option.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
        }
        /// <summary>
        /// Initializes the database by seeding data using the provided service provider.
        /// </summary>
        public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                await DbInitializer.DbInitializer.SeedDataAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                // في المشاريع الاحترافية يُفضل حقن ILogger وتسجيل الخطأ به بدلاً من Console
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            }
        }
        /// <summary>
        /// Adds rate limiting policies to the application.
        /// </summary>
        public static void AddLimiter(this WebApplicationBuilder builder)
        {
            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("SuggestionsPolicy", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 3,
                            Window = TimeSpan.FromHours(1),
                            QueueLimit = 0
                        }));

                options.AddPolicy("ReviewsPolicy", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromHours(1),
                            QueueLimit = 0
                        }));

                options.AddPolicy("VideoGuidesPolicy", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 3,
                            Window = TimeSpan.FromHours(1),
                            QueueLimit = 0
                        }));
            });
        }

        public static void AddCacheService(this WebApplicationBuilder builder)
        {
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString(nameof(StackExchange.Redis));
            });

            builder.Services.AddScoped<ICacheService, CacheService>();
        }
    }
    }
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.Areas.Users.Controllers
{
    [Area("Users")]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var platforms = await _mediator.Send(new ReadPlatformsQuery(), cancellationToken);
            var opportunities = await _mediator.Send(
                new ReadWorkOpportunitiesQuery(null, true, true),
                cancellationToken);
            var categories = await _mediator.Send(new ReadCategoriesQuery(), cancellationToken);

            var featured = platforms
                .Where(platform => platform.Status != PlatformStatus.Closed)
                .OrderByDescending(platform => platform.AverageRating)
                .ThenByDescending(platform => platform.ReviewCount)
                .ThenBy(platform => platform.Name)
                .Take(6)
                .ToList();

            if (featured.Count == 0)
            {
                featured = platforms
                    .OrderByDescending(platform => platform.CreatedAt)
                    .Take(6)
                    .ToList();
            }

            var categoryCounts = platforms
                .SelectMany(platform => platform.Categories)
                .GroupBy(category => category.CategoryId)
                .ToDictionary(group => group.Key, group => group.Count());

            ViewData["Title"] = "Discover AI Training Platforms";

            return View(new PublicHomeViewModel
            {
                FeaturedPlatforms = featured,
                LatestOpportunities = opportunities.Take(3).ToList(),
                Categories = categories,
                CategoryCounts = categoryCounts,
                TotalPlatforms = platforms.Count,
                TotalOpportunities = opportunities.Count
            });
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}

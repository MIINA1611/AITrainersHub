using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.Area.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var suggestions = await _mediator.Send(
                new ReadPlatformSuggestionsQuery(false),
                cancellationToken);

            var videoGuides = await _mediator.Send(
                new ReadPendingVideoGuidesQuery(),
                cancellationToken);

            var reviews = await _mediator.Send(
                new ReadPendingReviewsQuery(),
                cancellationToken);

            var opportunities = await _mediator.Send(
                new ReadPendingWorkOpportunitiesQuery(),
                cancellationToken);

            // The unfiltered query is used on purpose: platform commands invalidate
            // this cache key, while the status-filtered cache keys are not invalidated.
            var platforms = await _mediator.Send(
                new ReadPlatformsQuery(),
                cancellationToken);

            var leaders = await _userManager.GetUsersInRoleAsync(Helpers.LeaderRole);

            var model = new DashboardViewModel
            {
                PendingSuggestions = suggestions.Count,
                PendingVideoGuides = videoGuides.Count,
                PendingReviews = reviews.Count,
                PendingOpportunities = opportunities.Count,
                TotalLeaders = leaders.Count,
                TotalLivePlatforms = platforms.Count(platform => platform.Status == PlatformStatus.Open)
            };

            ViewData["Title"] = "Overview";
            return View(model);
        }
    }
}

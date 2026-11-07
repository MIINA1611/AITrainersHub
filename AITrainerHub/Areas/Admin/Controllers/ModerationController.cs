using AITrainerHub.Extensions;
using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Categories.Queries;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.PlatformSuggestion.Commands;
using AITrainersHub.Application.Features.PlatformSuggestion.Queries;
using AITrainersHub.Application.Features.Review.Commands;
using AITrainersHub.Application.Features.Review.Queries;
using AITrainersHub.Application.Features.VideoGuide.Commands;
using AITrainersHub.Application.Features.VideoGuide.Queries;
using AITrainersHub.Application.Features.WorkOpportunity.Commands;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using FluentValidation;
using huzcodes.Extensions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Area.Admin.Controllers
{
    public class ModerationController : AdminBaseController
    {
        private static readonly HashSet<string> AllowedTabs = new(StringComparer.OrdinalIgnoreCase)
        {
            "opportunities",
            "suggestions",
            "video-guides",
            "reviews"
        };

        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public ModerationController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? tab, CancellationToken cancellationToken)
        {
            var opportunities = await _mediator.Send(
                new ReadPendingWorkOpportunitiesQuery(),
                cancellationToken);

            var suggestions = await _mediator.Send(
                new ReadPlatformSuggestionsQuery(false),
                cancellationToken);

            var videoGuides = await _mediator.Send(
                new ReadPendingVideoGuidesQuery(),
                cancellationToken);

            var reviews = await _mediator.Send(
                new ReadPendingReviewsQuery(),
                cancellationToken);

            var model = new ModerationQueueViewModel
            {
                Opportunities = opportunities,
                Suggestions = suggestions,
                VideoGuides = videoGuides,
                Reviews = reviews,
                ActiveTab = tab is not null && AllowedTabs.Contains(tab) ? tab : "opportunities"
            };

            var leaders = await _userManager.GetUsersInRoleAsync(Helpers.LeaderRole);
            model.LeaderNames = leaders
                .GroupBy(leader => leader.Id)
                .ToDictionary(
                    group => group.Key,
                    group => group.First().DisplayName
                        ?? group.First().UserName
                        ?? group.First().Email
                        ?? "Unknown leader");

            var platforms = await _mediator.Send(new ReadPlatformsQuery(), cancellationToken);
            model.PlatformNames = platforms
                .GroupBy(platform => platform.Id)
                .ToDictionary(group => group.Key, group => group.First().Name);

            var categories = await _mediator.Send(new ReadCategoriesQuery(), cancellationToken);
            model.CategoryNames = categories
                .GroupBy(category => category.Id)
                .ToDictionary(group => group.Key, group => group.First().Name);

            ViewData["Title"] = "Moderation Queue";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ApproveOpportunity(Guid id, CancellationToken cancellationToken)
        {
            return RunAsync(
                async () => await _mediator.Send(
                    new UpdateWorkOpportunityCommand { Id = id, IsApproved = true },
                    cancellationToken),
                "Work opportunity approved successfully.",
                "opportunities");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> RejectOpportunity(Guid id, CancellationToken cancellationToken)
        {
            return RunAsync(
                async () => await _mediator.Send(
                    new DeleteWorkOpportunityCommand(id),
                    cancellationToken),
                "Work opportunity deleted successfully.",
                "opportunities");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ApproveSuggestion(Guid id, CancellationToken cancellationToken)
        {
            return RunAsync(
                async () => await _mediator.Send(
                    new UpdatePlatformSuggestionCommand { Id = id, IsReviewed = true },
                    cancellationToken),
                "Platform suggestion marked as reviewed.",
                "suggestions");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> RejectSuggestion(Guid id, CancellationToken cancellationToken)
        {
            return RunAsync(
                async () => await _mediator.Send(
                    new DeletePlatformSuggestionCommand(id),
                    cancellationToken),
                "Platform suggestion deleted successfully.",
                "suggestions");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ApproveVideoGuide(Guid id, CancellationToken cancellationToken)
        {
            return RunAsync(
                async () => await _mediator.Send(
                    new UpdateVideoGuideCommand { Id = id, IsApproved = true },
                    cancellationToken),
                "Video guide approved successfully.",
                "video-guides");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> RejectVideoGuide(Guid id, CancellationToken cancellationToken)
        {
            return RunAsync(
                async () => await _mediator.Send(
                    new DeleteVideoGuideCommand(id),
                    cancellationToken),
                "Video guide deleted successfully.",
                "video-guides");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> ApproveReview(Guid id, CancellationToken cancellationToken)
        {
            return RunAsync(
                async () => await _mediator.Send(
                    new UpdateReviewCommand { Id = id, IsApproved = true },
                    cancellationToken),
                "Review approved successfully.",
                "reviews");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> RejectReview(Guid id, CancellationToken cancellationToken)
        {
            return RunAsync(
                async () => await _mediator.Send(
                    new DeleteReviewCommand(id),
                    cancellationToken),
                "Review deleted successfully.",
                "reviews");
        }

        private async Task<IActionResult> RunAsync(
            Func<Task> action,
            string successMessage,
            string tab)
        {
            try
            {
                await action();
                TempData["Success"] = successMessage;
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.ToErrorMessage();
            }
            catch (ResultException exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Index), new { tab });
        }
    }
}

using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Area.Admin.Controllers
{
    [Area("Admin")]
    public class LeadersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public LeadersController(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var leaders = await _userManager.GetUsersInRoleAsync(Helpers.LeaderRole);
            var opportunities = await _mediator.Send(new ReadWorkOpportunitiesQuery(), cancellationToken);

            var opportunityCounts = opportunities
                .GroupBy(opportunity => opportunity.LeaderId)
                .ToDictionary(group => group.Key, group => group.Count());

            var model = new LeadersIndexViewModel
            {
                Leaders = leaders
                    .OrderBy(leader => leader.DisplayName ?? leader.Email)
                    .Select(leader => new LeaderRowViewModel
                    {
                        Id = leader.Id,
                        DisplayName = leader.DisplayName
                            ?? leader.UserName
                            ?? leader.Email
                            ?? "Unnamed leader",
                        Email = leader.Email ?? string.Empty,
                        RegisteredAt = leader.CreatedAt,
                        OpportunitiesCount = opportunityCounts.GetValueOrDefault(leader.Id),
                        IsActive = leader.IsActive
                    })
                    .ToList()
            };

            ViewData["Title"] = "Leaders";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            var leader = await _userManager.FindByIdAsync(id.ToString());

            if (leader is null)
            {
                TempData["Error"] = "Leader not found.";
                return RedirectToAction(nameof(Index));
            }

            if (leader.IsActive)
            {
                leader.IsActive = false;
                leader.LockoutEnabled = true;
                leader.LockoutEnd = DateTimeOffset.MaxValue;
            }
            else
            {
                leader.IsActive = true;
                leader.LockoutEnabled = true;
                leader.LockoutEnd = null;
            }

            var result = await _userManager.UpdateAsync(leader);

            if (!result.Succeeded)
            {
                TempData["Error"] = string.Join(
                    " ",
                    result.Errors.Select(error => error.Description));
                return RedirectToAction(nameof(Index));
            }

            // Invalidate any authentication cookie that is already signed in.
            await _userManager.UpdateSecurityStampAsync(leader);

            TempData["Success"] = leader.IsActive
                ? "Leader activated successfully."
                : "Leader deactivated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}

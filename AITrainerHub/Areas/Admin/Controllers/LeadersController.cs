using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Area.Admin.Controllers
{
    public class LeadersController : AdminBaseController
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
            var leaders = await _userManager.GetUsersInRoleAsync(AITrainersHub.Domain.Enums.Helpers.LeaderRole);
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

            ViewData["Title"] = "Manage Leaders";
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Leader";
            return View(new CreateLeaderViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLeaderViewModel model)
        {
            ViewData["Title"] = "Create Leader";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim();

            if (await _userManager.FindByEmailAsync(email) is not null)
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email address already exists.");
                return View(model);
            }

            var leader = new ApplicationUser
            {
                DisplayName = model.DisplayName.Trim(),
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(leader, model.Password);

            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            var roleResult = await _userManager.AddToRoleAsync(leader, AITrainersHub.Domain.Enums.Helpers.LeaderRole);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(leader);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            TempData["Success"] = $"Leader account for {leader.DisplayName} was created successfully.";
            return RedirectToAction(nameof(Index));
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

using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Areas.Leader.Controllers
{
    [Authorize(Roles = Helpers.LeaderRole)]
    public class HomeController : LeaderControllerBase
    {
        private readonly IMediator _mediator;

        public HomeController(
            IMediator mediator,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
            : base(userManager, signInManager)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var opportunities = await _mediator.Send(
                new ReadWorkOpportunitiesQuery(LeaderId),
                cancellationToken);

            ViewData["Title"] = "Leader Dashboard";

            return View(new LeaderDashboardViewModel
            {
                Opportunities = opportunities,
                DisplayName = CurrentLeader.DisplayName
                    ?? CurrentLeader.UserName
                    ?? CurrentLeader.Email
                    ?? "Leader"
            });
        }
    }
}

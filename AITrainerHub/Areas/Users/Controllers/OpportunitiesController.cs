using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Platform.Queries;
using AITrainersHub.Application.Features.WorkOpportunity.DTOs;
using AITrainersHub.Application.Features.WorkOpportunity.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Areas.Users.Controllers
{
    [Area("Users")]
    public class OpportunitiesController : Controller
    {
        private const int PageSize = 9;

        private readonly IMediator _mediator;

        public OpportunitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? platform,
            string? search,
            int page = 1,
            CancellationToken cancellationToken = default)
        {
            var opportunities = await _mediator.Send(
                new ReadWorkOpportunitiesQuery(null, true, true),
                cancellationToken);
            var platforms = await _mediator.Send(new ReadPlatformsQuery(), cancellationToken);

            IEnumerable<WorkOpportunityDto> filtered = opportunities;

            if (!string.IsNullOrWhiteSpace(platform))
            {
                if (Guid.TryParse(platform, out var platformId))
                {
                    filtered = filtered.Where(opportunity => opportunity.PlatformId == platformId);
                }
                else
                {
                    filtered = filtered.Where(opportunity =>
                        string.Equals(
                            opportunity.PlatformSlug,
                            platform,
                            StringComparison.OrdinalIgnoreCase));
                }
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                filtered = filtered.Where(opportunity =>
                    opportunity.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    opportunity.DescriptionAr.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    opportunity.DescriptionEn.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    opportunity.LeaderDisplayName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    opportunity.PlatformName.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            var filteredList = filtered
                .OrderByDescending(opportunity => opportunity.CreatedAt)
                .ToList();

            if (page < 1)
            {
                page = 1;
            }

            var totalPages = filteredList.Count == 0
                ? 1
                : (int)Math.Ceiling(filteredList.Count / (double)PageSize);

            if (page > totalPages)
            {
                page = totalPages;
            }

            ViewData["Title"] = "Work Opportunities";

            return View(new OpportunitiesIndexViewModel
            {
                Opportunities = filteredList
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                Platforms = platforms.OrderBy(item => item.Name).ToList(),
                Platform = platform,
                Search = search,
                Page = page,
                PageSize = PageSize,
                TotalCount = filteredList.Count,
                TotalPages = totalPages
            });
        }
    }
}

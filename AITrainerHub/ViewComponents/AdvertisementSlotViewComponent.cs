using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Advertisement.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.ViewComponents
{
    /// <summary>
    /// Renders the newest active, date-valid advertisement for a named slot.
    /// Date validity is applied here because the advertisement query only
    /// filters on the active flag.
    /// </summary>
    public class AdvertisementSlotViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public AdvertisementSlotViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string position)
        {
            var advertisements = await _mediator.Send(new ReadAdvertisementsQuery(true));
            var today = DateTime.UtcNow.Date;

            var advertisement = advertisements
                .Where(item => string.Equals(item.Position?.Trim(), position, StringComparison.OrdinalIgnoreCase))
                .Where(item =>
                    (!item.StartDate.HasValue || item.StartDate.Value.Date <= today) &&
                    (!item.EndDate.HasValue || item.EndDate.Value.Date >= today))
                .OrderByDescending(item => item.CreatedAt)
                .FirstOrDefault();

            return View(new AdvertisementSlotViewModel
            {
                Position = position,
                Advertisement = advertisement
            });
        }
    }
}

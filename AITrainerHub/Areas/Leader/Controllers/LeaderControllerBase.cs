using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AITrainerHub.Areas.Leader.Controllers
{
    /// <summary>
    /// Base controller for every Leader page. In addition to the role check it
    /// verifies on each request that the account is still active and not locked,
    /// so deactivating a leader takes effect immediately.
    /// </summary>
    [Area("Leader")]
    [Authorize(Roles = Helpers.LeaderRole)]
    public abstract class LeaderControllerBase : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        protected LeaderControllerBase(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        protected ApplicationUser CurrentLeader { get; private set; } = null!;

        protected Guid LeaderId => CurrentLeader.Id;

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var user = await _userManager.GetUserAsync(User);
            var isLockedOut = user?.LockoutEnd.HasValue == true &&
                              user.LockoutEnd.Value > DateTimeOffset.UtcNow;

            if (user is null || !user.IsActive || isLockedOut)
            {
                await _signInManager.SignOutAsync();
                TempData["Error"] = "Your leader account is inactive or locked. Please contact an administrator.";
                context.Result = RedirectToPage("/Account/Login", new { area = "Identity" });
                return;
            }

            CurrentLeader = user;
            await next();
        }
    }
}

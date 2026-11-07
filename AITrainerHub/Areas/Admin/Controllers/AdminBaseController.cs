using AITrainersHub.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AITrainerHub.Area.Admin.Controllers
{
    /// <summary>
    /// Base controller for every page in the Admin Area. The Admin role is
    /// enforced here so a newly added admin controller cannot accidentally be
    /// exposed to anonymous visitors or non-admin accounts.
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = Helpers.AdminRole)]
    public abstract class AdminBaseController : Controller
    {
    }
}

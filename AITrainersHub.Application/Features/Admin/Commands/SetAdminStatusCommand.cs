using MediatR;

namespace AITrainersHub.Application.Features.Admin.Commands
{
    /// <summary>
    /// Activates or deactivates an administrator account.
    /// </summary>
    public class SetAdminStatusCommand : IRequest<Unit>
    {
        public Guid AdminId { get; set; }
        public Guid CurrentAdminId { get; set; }
        public bool IsActive { get; set; }
    }
}

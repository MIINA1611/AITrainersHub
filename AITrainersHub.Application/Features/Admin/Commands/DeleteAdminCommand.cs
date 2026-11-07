using MediatR;

namespace AITrainersHub.Application.Features.Admin.Commands
{
    /// <summary>
    /// Deletes an administrator account.
    /// </summary>
    public class DeleteAdminCommand : IRequest<Unit>
    {
        public Guid AdminId { get; set; }
        public Guid CurrentAdminId { get; set; }
    }
}

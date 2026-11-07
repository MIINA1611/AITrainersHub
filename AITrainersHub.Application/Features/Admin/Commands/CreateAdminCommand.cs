using AITrainersHub.Application.Features.Admin.DTOs;
using MediatR;

namespace AITrainersHub.Application.Features.Admin.Commands
{
    /// <summary>
    /// Creates a new administrator account with the Admin role.
    /// </summary>
    public class CreateAdminCommand : IRequest<AdminDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

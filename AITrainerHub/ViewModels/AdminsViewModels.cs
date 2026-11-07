using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data required by the admin account management page.
    /// </summary>
    public class AdminsIndexViewModel
    {
        public List<AdminRowViewModel> Admins { get; set; } = new();
        public Guid CurrentAdminId { get; set; }
    }

    /// <summary>
    /// One row in the admin accounts table.
    /// </summary>
    public class AdminRowViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrentUser { get; set; }
    }

    /// <summary>
    /// Form used by an administrator to create another Admin account.
    /// </summary>
    public class CreateAdminViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        [Display(Name = "Full name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(200, ErrorMessage = "Email address must not exceed 200 characters.")]
        [Display(Name = "Email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Temporary password")]
        public string Password { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data required by the leaders management page.
    /// </summary>
    public class LeadersIndexViewModel
    {
        public List<LeaderRowViewModel> Leaders { get; set; } = new();
    }

    /// <summary>
    /// One row in the leaders table.
    /// </summary>
    public class LeaderRowViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public int OpportunitiesCount { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Form used by an administrator to create a Leader account.
    /// </summary>
    public class CreateLeaderViewModel
    {
        [Required(ErrorMessage = "Display name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Display name must be between 2 and 100 characters.")]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; } = string.Empty;

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

        [Required(ErrorMessage = "Please confirm the password.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

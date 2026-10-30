using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.Request_Models
{
    /// <summary>
    /// Form model used by the Categories Create/Edit Razor views.
    /// The Application layer commands have parameterized constructors only,
    /// so the controller maps this model to the existing MediatR requests.
    /// </summary>
    public class CategoryFormRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name must not exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string? Description { get; set; }
    }
}

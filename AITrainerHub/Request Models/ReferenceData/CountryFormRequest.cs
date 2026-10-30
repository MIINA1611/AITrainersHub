using System.ComponentModel.DataAnnotations;

namespace AITrainerHub.Request_Models
{
    /// <summary>
    /// Form model used by the Reference Data country create/edit forms.
    /// </summary>
    public class CountryFormRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Arabic name is required.")]
        [StringLength(100, ErrorMessage = "Arabic name must not exceed 100 characters.")]
        public string NameAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "English name is required.")]
        [StringLength(100, ErrorMessage = "English name must not exceed 100 characters.")]
        public string NameEn { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country code is required.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must consist of exactly 2 characters.")]
        public string IsoCode { get; set; } = string.Empty;
    }
}

using AITrainerHub.Request_Models;
using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.Platform.DTOs;
using AITrainersHub.Application.Features.Review.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data required by the platform edit page, including the main form,
    /// linked collections and the reference data used by the add dropdowns.
    /// </summary>
    public class PlatformEditViewModel
    {
        public PlatformFormRequest Form { get; set; } = new();
        public PlatformDetailsDto Details { get; set; } = new();

        /// <summary>
        /// All reviews for the platform (approved and pending), unlike the
        /// details DTO which only contains approved reviews.
        /// </summary>
        public List<ReviewDto> Reviews { get; set; } = new();

        public List<CategoryListResponse> AllCategories { get; set; } = new();
        public List<CountryDto> AllCountries { get; set; } = new();
        public List<PaymentMethodDto> AllPaymentMethods { get; set; } = new();
    }
}

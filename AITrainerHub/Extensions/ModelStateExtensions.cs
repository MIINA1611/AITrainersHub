using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AITrainerHub.Extensions
{
    /// <summary>
    /// Maps FluentValidation failures raised by the MediatR pipeline into MVC ModelState.
    /// </summary>
    public static class ModelStateExtensions
    {
        public static void AddToModelState(
            this ValidationResult result,
            ModelStateDictionary modelState,
            string? prefix = null)
        {
            foreach (var error in result.Errors)
            {
                var propertyName = string.IsNullOrWhiteSpace(error.PropertyName)
                    ? string.Empty
                    : error.PropertyName;

                var key = string.IsNullOrWhiteSpace(prefix)
                    ? propertyName
                    : string.IsNullOrWhiteSpace(propertyName)
                        ? prefix
                        : $"{prefix}.{propertyName}";

                modelState.AddModelError(key, error.ErrorMessage);
            }
        }

        public static void AddValidationErrors(this ModelStateDictionary modelState, ValidationException exception)
        {
            foreach (var failure in exception.Errors)
            {
                var key = string.IsNullOrWhiteSpace(failure.PropertyName)
                    ? string.Empty
                    : failure.PropertyName;

                modelState.AddModelError(key, failure.ErrorMessage);
            }
        }

        public static string ToErrorMessage(this ValidationException exception)
        {
            var messages = exception.Errors
                .Select(error => error.ErrorMessage)
                .Where(message => !string.IsNullOrWhiteSpace(message))
                .Distinct()
                .ToList();

            return messages.Count == 0
                ? "The submitted values are invalid."
                : string.Join(" ", messages);
        }
    }
}

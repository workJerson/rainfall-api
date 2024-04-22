using FluentValidation;
using rainfall_api.Services;

namespace rainfall_api.Validators
{
    public class GetRainfallReadingsValidator : AbstractValidator<GetRainfallReadingsRequest>
    {
        public GetRainfallReadingsValidator()
        {
            RuleFor(i => i.StationId)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");

            RuleFor(i => i.Count)
                .GreaterThanOrEqualTo(1)
                .WithMessage("{PropertyName} must be greater than or equal to 1.")
                .LessThanOrEqualTo(100)
                .WithMessage("{PropertyName} should be less than or equal to 100.");
        }
    }
}

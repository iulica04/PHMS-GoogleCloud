using FluentValidation;

namespace Application.Use_Cases.Commands.TreatmentCommands
{
    public abstract class TreatmentCommandValidator<T, U>: AbstractValidator<T> where T : TreatmentCommand<U>
    {
        protected TreatmentCommandValidator()
        {
            //RuleFor(x => x.Type)
            //    .NotEmpty().WithMessage("Type is required.")
            //    .Must(type => type == "Drug" || type == "NaturalRemedy" || type == "Surgery" || type == "Admission")
            //    .WithMessage("Type must be one of the following: 'Drug', 'NaturalRemedy', 'Surgery', 'Admission'.");

         

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("StartDate cannot be in the future.");

            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("Duration is required.")
                .GreaterThan(x => x.StartDate).WithMessage("Duration must be after StartDate.");

        }

        protected static bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}

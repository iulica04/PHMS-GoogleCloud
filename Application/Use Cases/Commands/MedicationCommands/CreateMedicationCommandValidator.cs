using FluentValidation;
using Application.Commands.MedicationCommand;

namespace Application.Validators
{
    public class CreateMedicationCommandValidator : AbstractValidator<CreateMedicationCommand>
    {
        public CreateMedicationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Type)
                .Must(type => Enum.IsDefined(type))
                .IsInEnum().WithMessage("Type must be a valid MedicationType.");

            RuleFor(x => x.Ingredients)
                .NotEmpty().WithMessage("Ingredients are required.");

            RuleFor(x => x.AdverseEffects)
                .NotEmpty().WithMessage("AdverseEffects are required.");
               
        }
    }
}
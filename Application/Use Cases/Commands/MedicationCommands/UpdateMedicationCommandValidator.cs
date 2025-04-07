using FluentValidation;

namespace Application.Commands.MedicationCommand
{
    public class UpdateMedicationCommandValidator : AbstractValidator<UpdateMedicationCommand>
    {
        public UpdateMedicationCommandValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(BeAValidGuid).WithMessage("Invalid Id format.");
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

        private static bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}

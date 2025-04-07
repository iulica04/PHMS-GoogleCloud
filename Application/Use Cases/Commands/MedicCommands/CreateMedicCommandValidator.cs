using FluentValidation;

namespace Application.Commands.Medic
{
    public class CreateMedicCommandValidator : UserCommandValidator<CreateMedicCommand, Guid>
    {
        public CreateMedicCommandValidator()
        {
            RuleFor(x => x.Rank)
                .NotNull().WithMessage("Rank is required.")
                .MaximumLength(30).WithMessage("Rank must be at most 30 characters.");

            RuleFor(x => x.Specialization)
                .NotNull().WithMessage("Specialization is required.")
                .MaximumLength(30).WithMessage("Specialization must be at most 30 characters.");

            RuleFor(x => x.Hospital)
                .NotNull().WithMessage("Hospital is required.")
                .MaximumLength(30).WithMessage("Hospital must be at most 30 characters.");
        }
    }
}

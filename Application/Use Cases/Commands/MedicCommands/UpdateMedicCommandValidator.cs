using FluentValidation;
using MediatR;

namespace Application.Commands.Medic
{
    public class UpdateMedicCommandValidator : UserCommandValidator<UpdateMedicCommand, Unit>
    {
        public UpdateMedicCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(BeAValidGuid).WithMessage("Invalid Id format.");

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

        private static bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}

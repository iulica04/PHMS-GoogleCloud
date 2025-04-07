using FluentValidation;
using MediatR;

namespace Application.Commands.Patient
{
    public class UpdatePatientCommandValidator : UserCommandValidator<UpdatePatientCommand, Unit>
    {
        public UpdatePatientCommandValidator(): base()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(BeAValidGuid).WithMessage("Invalid Id format.");
        }

        private static bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}

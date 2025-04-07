using FluentValidation;

namespace Application.Commands.Patient
{
    public class DeletePatientByIdCommandValidator : AbstractValidator<DeletePatientByIdCommand>
    {
        public DeletePatientByIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}

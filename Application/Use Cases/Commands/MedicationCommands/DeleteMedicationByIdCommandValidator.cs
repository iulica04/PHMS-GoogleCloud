using FluentValidation;

namespace Application.Commands.MedicationCommand
{
    public class DeleteMedicationByIdCommandValidator : AbstractValidator<DeleteMedicationByIdCommand>
    {
        public DeleteMedicationByIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}

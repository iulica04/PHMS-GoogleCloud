using FluentValidation;

namespace Application.Commands.TreatmentCommands
{
    public class DeleteTreatmentCommandByIdValidator : AbstractValidator<DeleteTreatmentByIdCommand>
    {
        public DeleteTreatmentCommandByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}

using FluentValidation;

namespace Application.Commands.MedicalConditionCommands
{
    public class DeleteMedicalConditionByIdCommandValidator : AbstractValidator<DeleteMedicalConditionByIdCommand>
    {
        public DeleteMedicalConditionByIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}

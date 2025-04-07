using FluentValidation;

namespace Application.Commands.Medic
{
    public class DeleteMedicByIdCommandValidator : AbstractValidator<DeleteMedicByIdCommand>
    {
        public DeleteMedicByIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}

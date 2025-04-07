using FluentValidation;

namespace Application.Commands.Administrator
{
    public class DeleteAdminCommandByIdValidator : AbstractValidator<DeleteAdminByIdCommand>
    {
        public DeleteAdminCommandByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}

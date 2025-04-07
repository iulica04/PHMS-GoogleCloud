using Application.Commands.Administrator;
using FluentValidation;
using MediatR;

namespace Application.Commands.Admin
{
    public class UpdateAdminCommandValidator : UserCommandValidator<UpdateAdminCommand, Unit>
    {
        public UpdateAdminCommandValidator()
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

using Application.Use_Cases.Commands.MedicalConditionCommands;
using FluentValidation;
using MediatR;

namespace Application.Commands.MedicalConditionCommands
{
    public class UpdateMedicalConditionCommandValidator : MedicalConditionCommandValidator<UpdateMedicalConditionCommand, Unit>
    {
        public UpdateMedicalConditionCommandValidator()
        {
            RuleFor(x => x.MedicalConditionId)
                .NotEmpty().WithMessage("MedicalConditionId is required.")
                .Must(BeAValidGuid).WithMessage("Invalid MedicalConditionId format.");
        }
    }
}

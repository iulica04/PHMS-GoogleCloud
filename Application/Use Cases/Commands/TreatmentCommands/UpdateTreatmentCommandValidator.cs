using Application.Use_Cases.Commands.TreatmentCommands;
using FluentValidation;
using MediatR;

namespace Application.Commands.TreatmentCommands
{
    public class UpdateTreatmentCommandValidator : TreatmentCommandValidator<UpdateTreatmentCommand, Unit>
    {
        public UpdateTreatmentCommandValidator()
        {
            RuleFor(x => x.TreatmentId)
                .NotEmpty().WithMessage("Id is required.")
                .Must(BeAValidGuid).WithMessage("Invalid Id format.");
        }
    }
}

using Application.Use_Cases.Commands.MedicalConditionCommands;
using MediatR;

namespace Application.Commands.MedicalConditionCommands
{
    public class UpdateMedicalConditionCommand : MedicalConditionCommand<Unit>
    {
        public Guid MedicalConditionId { get; set; }
    }
}

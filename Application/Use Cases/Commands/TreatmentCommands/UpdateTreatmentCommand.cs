using Application.Use_Cases.Commands.TreatmentCommands;
using MediatR;

namespace Application.Commands.TreatmentCommands
{
    public class UpdateTreatmentCommand : TreatmentCommand<Unit>
    {
        public Guid TreatmentId { get; set; }
    }
}

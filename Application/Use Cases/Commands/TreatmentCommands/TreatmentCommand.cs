using Application.DTOs;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Use_Cases.Commands.TreatmentCommands
{
    public abstract class TreatmentCommand<T>: IRequest<Result<T>>
    {
        public required TreatmentType Type { get; set; }
        public required string Name { get; set; }
        public Guid MedicalConditionId { get; set; }
        public required string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Duration { get; set; }
        public required List<MedicationDto> Medications { get; set; }
    }
}

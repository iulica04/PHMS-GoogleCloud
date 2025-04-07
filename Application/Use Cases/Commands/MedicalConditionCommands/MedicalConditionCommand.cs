using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands.MedicalConditionCommands
{
    public abstract class MedicalConditionCommand<T>: IRequest<Result<T>>
    {
        public Guid PatientId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required string CurrentStatus { get; set; }
        public required bool IsGenetic { get; set; }
        public required string Recommendation { get; set; }
        public required List<TreatmentDto> Treatments { get; set; }
    }
}

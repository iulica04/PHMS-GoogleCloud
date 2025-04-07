using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Use_Cases.Commands.ConsultationCommands
{
    public class CreateConsultationCommand : IRequest<Result<Guid>>
    {
        public required Guid PatientId { get; set; }

        public required Guid MedicId { get; set; }
        public ConsultationStatus Status { get; set; } = ConsultationStatus.Pending;
        public required DateTime Date { get; set; }
        public required string Location { get; set; }
    }
}

using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Use_Cases.Commands.ConsultationCommands
{
    public class UpdateConsultationCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
        public ConsultationStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
    }
}

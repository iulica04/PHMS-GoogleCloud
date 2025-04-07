using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands.PatientCommands
{
    public class UpdatePatientPasswordCommand : IRequest<Result<Unit>>
    {
        public Guid PatientId { get; set; }
        public required string Password { get; set; }
    }
}

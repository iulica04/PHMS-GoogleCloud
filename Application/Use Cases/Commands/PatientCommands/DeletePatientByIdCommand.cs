using Domain.Common;
using MediatR;

namespace Application.Commands.Patient
{
    public record DeletePatientByIdCommand(Guid Id) : IRequest<Result<Unit>>;
    
}

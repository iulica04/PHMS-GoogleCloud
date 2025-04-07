using Domain.Common;
using MediatR;

namespace Application.Commands.TreatmentCommands
{
    public record DeleteTreatmentByIdCommand(Guid Id) : IRequest<Result<Unit>>;
}

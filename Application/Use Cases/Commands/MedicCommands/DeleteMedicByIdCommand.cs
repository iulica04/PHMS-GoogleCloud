using Domain.Common;
using MediatR;

namespace Application.Commands.Medic
{
    public record DeleteMedicByIdCommand(Guid Id) : IRequest<Result<Unit>>;
}

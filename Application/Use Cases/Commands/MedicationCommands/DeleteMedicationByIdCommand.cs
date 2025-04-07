using Domain.Common;
using MediatR;

namespace Application.Commands.MedicationCommand
{
    public record DeleteMedicationByIdCommand(Guid Id) : IRequest<Result<Unit>>;
}
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands.ConsultationCommands
{
    public record DeleteConsultationCommand(Guid Id) : IRequest<Result<Unit>>;
}

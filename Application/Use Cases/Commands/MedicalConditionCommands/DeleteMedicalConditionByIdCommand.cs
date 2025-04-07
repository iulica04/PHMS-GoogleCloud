using Domain.Common;
using MediatR;

namespace Application.Commands.MedicalConditionCommands
{
    public class DeleteMedicalConditionByIdCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }
}

using Application.Commands.MedicalConditionCommands;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers.MedicalConditionCommandHandlers
{
    public class DeleteMedicalConditionByIdCommandHandler : IRequestHandler<DeleteMedicalConditionByIdCommand, Result<Unit>>
    {
        private readonly IMedicalConditionRepository repository;
        public DeleteMedicalConditionByIdCommandHandler(IMedicalConditionRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<Unit>> Handle(DeleteMedicalConditionByIdCommand request, CancellationToken cancellationToken)
        {
            var medicalCondition = await repository.GetByIdAsync(mc => mc.MedicalConditionId == request.Id);
            if (medicalCondition == null)
            {
                return Result<Unit>.Failure("Medical condition not found");
            }
            await repository.DeleteAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}

using Application.Commands.TreatmentCommands;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers.TreatmentCommandHandler
{
    public class DeleteTreatmentByIdCommandHandler : IRequestHandler<DeleteTreatmentByIdCommand, Result<Unit>>
    {
        private readonly ITreatmentRepository repository;
        public DeleteTreatmentByIdCommandHandler(ITreatmentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<Unit>> Handle(DeleteTreatmentByIdCommand request, CancellationToken cancellationToken)
        {
            var treatment = await repository.GetByIdAsync(request.Id);
            if (treatment == null)
            {
                return Result<Unit>.Failure("Treatment not found");
            }
            await repository.DeleteAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}

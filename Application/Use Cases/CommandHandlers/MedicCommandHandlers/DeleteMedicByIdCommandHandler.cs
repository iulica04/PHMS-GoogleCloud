using Application.Commands.Medic;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers.MedicCommandHandlers
{
    public class DeleteMedicByIdCommandHandler : IRequestHandler<DeleteMedicByIdCommand, Result<Unit>>
    {
        private readonly IMedicRepository repository;

        public DeleteMedicByIdCommandHandler(IMedicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<Unit>> Handle(DeleteMedicByIdCommand request, CancellationToken cancellationToken)
        {
            var medic = await repository.GetByIdAsync(request.Id);
            if (medic == null)
            {
                return Result<Unit>.Failure($"Medic with id {request.Id} not found");
            }
            await repository.DeleteAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}

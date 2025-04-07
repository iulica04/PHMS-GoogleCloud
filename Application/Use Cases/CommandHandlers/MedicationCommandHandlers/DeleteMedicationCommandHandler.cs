using Application.Commands.MedicationCommand;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers.MedicationCommandHandlers
{
    public class DeleteMedicationCommandHandler : IRequestHandler<DeleteMedicationByIdCommand, Result<Unit>>
    {
        private readonly IMedicationRepository repository;
        public DeleteMedicationCommandHandler(IMedicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<Unit>> Handle(DeleteMedicationByIdCommand request, CancellationToken cancellationToken)
        {
            var medication = await repository.GetByIdAsync(request.Id);
            if (medication == null)
            {
                return Result<Unit>.Failure("Medication not found");
            }
            await repository.DeleteAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}

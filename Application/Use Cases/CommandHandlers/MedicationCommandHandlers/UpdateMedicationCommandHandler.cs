using Application.Commands.MedicationCommand;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers.MedicationCommandHandlers
{
    public class UpdateMedicationCommandHandler : IRequestHandler<UpdateMedicationCommand, Result<Unit>>
    {
        private readonly IMedicationRepository repository;
        private readonly IMapper mapper;

        public UpdateMedicationCommandHandler(IMedicationRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateMedicationCommand request, CancellationToken cancellationToken)
        {
            var medication = await repository.GetByIdAsync(request.Id);
            if (medication == null)
            {
                return Result<Unit>.Failure("Medication not found.");
            }

            mapper.Map(request, medication);

            await repository.UpdateAsync(medication);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
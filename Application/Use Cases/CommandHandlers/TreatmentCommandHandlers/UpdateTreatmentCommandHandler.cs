using Application.Commands.TreatmentCommands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers.TreatmentCommandHandlers
{
    public class UpdateTreatmentCommandHandler : IRequestHandler<UpdateTreatmentCommand, Result<Unit>>
    {
        private readonly ITreatmentRepository repository;
        private readonly IMapper mapper;
        public UpdateTreatmentCommandHandler(ITreatmentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<Unit>> Handle(UpdateTreatmentCommand request, CancellationToken cancellationToken)
        {
            var treatment = await repository.GetByIdAsync(request.TreatmentId);
            if (treatment == null)
            {
                return Result<Unit>.Failure("Treatment not found");
            }
            treatment = mapper.Map(request, treatment);
            await repository.UpdateAsync(treatment);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}

using Application.Commands.MedicalConditionCommands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers.MedicalConditionCommandHandlers
{
    public class UpdateMedicalConditionCommandHandler : IRequestHandler<UpdateMedicalConditionCommand, Result<Unit>>
    {
        private readonly IMedicalConditionRepository repository;
        private readonly IMapper mapper;

        public UpdateMedicalConditionCommandHandler(IMedicalConditionRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateMedicalConditionCommand request, CancellationToken cancellationToken)
        {
            var medicalCondition = await repository.GetByIdAsync(mc => mc.MedicalConditionId == request.MedicalConditionId);
            if (medicalCondition == null)
            {
                return Result<Unit>.Failure("Medical condition not found");
            }
            mapper.Map(request, medicalCondition);
            await repository.UpdateAsync(medicalCondition);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}

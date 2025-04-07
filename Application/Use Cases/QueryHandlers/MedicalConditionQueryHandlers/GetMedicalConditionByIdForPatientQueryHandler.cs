using Application.DTOs;
using Application.Queries.MedialConditionQueries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers.MedicalConditionQueryHandlers
{
    public class GetMedicalConditionByIdForPatientQueryHandler : IRequestHandler<GetMedicalConditionByIdQuery, MedicalConditionDto?>
    {
        private readonly IMedicalConditionRepository repository;
        private readonly IMapper mapper;
        public GetMedicalConditionByIdForPatientQueryHandler(IMedicalConditionRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<MedicalConditionDto?> Handle(GetMedicalConditionByIdQuery request, CancellationToken cancellationToken)
        {
            var medicalCondition = await repository.GetByIdAsync(mc => mc.PatientId == request.PatientId && mc.MedicalConditionId == request.MedicalConditionId);

            if (medicalCondition == null)
                return null;

            return mapper.Map<MedicalConditionDto>(medicalCondition);
        }
    }
}

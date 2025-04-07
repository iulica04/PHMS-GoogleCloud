using Application.DTOs;
using Application.Use_Cases.Queries.MedicationQueries;
using AutoMapper;
using Domain.Services;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.MedicationQueryHandlers
{
    public class GetExternalMedicationByConditionQueryHandler : IRequestHandler<GetExternalMedicationByConditionQuery, List<ExternalMedicationDto>>
    {
        private readonly IMedicationExternalService medicationExternalService;
        private readonly IMapper mapper;

        public GetExternalMedicationByConditionQueryHandler(IMedicationExternalService medicationExternalService, IMapper mapper)
        {
            this.medicationExternalService = medicationExternalService;
            this.mapper = mapper;
        }

        public async Task<List<ExternalMedicationDto>> Handle(GetExternalMedicationByConditionQuery request, CancellationToken cancellationToken)
        {
            var medications = await medicationExternalService.GetMedicationsByConditionAsync(request.Condition);
            return mapper.Map<List<ExternalMedicationDto>>(medications);
        }
    }
}

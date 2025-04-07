using Application.DTOs;
using Application.Queries.MedicationQueries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers.MedicationQueryHandlers
{
    public class GetAllMedicationsQueryHandler : IRequestHandler<GetAllMedicationsQuery, List<MedicationDto>>
    {
        private readonly IMedicationRepository repository;
        private readonly IMapper mapper;
        public GetAllMedicationsQueryHandler(IMedicationRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<List<MedicationDto>> Handle(GetAllMedicationsQuery request, CancellationToken cancellationToken)
        {
            var medications = await repository.GetAllAsync();
            return mapper.Map<List<MedicationDto>>(medications);
        }
    }
}

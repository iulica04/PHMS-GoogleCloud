using Application.DTOs;
using Application.Queries.MedicationQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers.MedicationQueryHandlers
{
    public class GetMedicationByIdQueryHandler : IRequestHandler<GetMedicationByIdQuery, Result<MedicationDto>>
    {
        private readonly IMedicationRepository repository;
        private readonly IMapper mapper;
        public GetMedicationByIdQueryHandler(IMedicationRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<MedicationDto>> Handle(GetMedicationByIdQuery request, CancellationToken cancellationToken)
        {
            var medication = await repository.GetByIdAsync(request.MedicationId);
            if (medication == null)
            {
                return Result<MedicationDto>.Failure("Medication not found");
            }
            return Result<MedicationDto>.Success(mapper.Map<MedicationDto>(medication));
        }
    }
}

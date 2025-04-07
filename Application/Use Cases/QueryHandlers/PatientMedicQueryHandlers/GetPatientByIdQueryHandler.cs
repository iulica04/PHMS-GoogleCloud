using Application.DTOs;
using Application.Queries.PatientQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers
{
    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, Result<PatientDto>>
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;
        public GetPatientByIdQueryHandler(IPatientRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<PatientDto>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await repository.GetByIdAsync(request.Id);
            if (patient == null)
            {
                return Result<PatientDto>.Failure("Patient not found");
            }
            return Result<PatientDto>.Success(mapper.Map<PatientDto>(patient));
        }
    }
}

using Application.DTOs;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Gridify;
using Application.Queries;

namespace Application.QueryHandlers.PatientMedicQueryHandlers
{
    public class GetFilteredPatientsQueryHandler : IRequestHandler<GetFilteredQuery<Patient, PatientDto>, Result<PagedResult<PatientDto>>>
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;

        public GetFilteredPatientsQueryHandler(IPatientRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<PagedResult<PatientDto>>> Handle(GetFilteredQuery<Patient, PatientDto> request, CancellationToken cancellationToken)
        {
            var patients = await repository.GetAllAsync();
            if (request.Filter != null)
            {
                patients = patients.AsQueryable().Where(request.Filter);
            }

            var totalCount = patients.Count();
            var pagedPatients = patients.AsQueryable().ApplyPaging(request.Page, request.PageSize);

            var patientDtos = mapper.Map<List<PatientDto>>(pagedPatients);
            var pagedResult = new PagedResult<PatientDto>(patientDtos, totalCount);

            return Result<PagedResult<PatientDto>>.Success(pagedResult);
        }
    }
}

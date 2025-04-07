using Application.DTOs;
using Application.Queries.PatientQueries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers.PatientMedicQueryHandlers
{
    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<PatientDto>>
    {
        private readonly IPatientRepository patientRepository;
        private readonly IMapper mapper;

        public GetAllPatientsQueryHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            this.patientRepository = patientRepository;
            this.mapper = mapper;
        }

        public async Task<List<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await patientRepository.GetAllAsync();
            return mapper.Map<List<PatientDto>>(patients);
        }

    }
}

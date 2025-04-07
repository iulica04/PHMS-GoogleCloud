using Application.DTOs;
using Application.Use_Cases.Queries.ConsultationsQueries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.ConsultationsQueryHandlers
{
    public class GetAllConsultationsQueryHandler : IRequestHandler<GetAllConsultationsQuery, List<ConsultationDto>>
    {
        private readonly IConsultationRepository repository;
        private readonly IMapper mapper;

        public GetAllConsultationsQueryHandler(IConsultationRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<List<ConsultationDto>> Handle(GetAllConsultationsQuery request, CancellationToken cancellationToken)
        {
            var consultations = await repository.GetAllAsync();
            return mapper.Map<List<ConsultationDto>>(consultations);
        }
    }
}

using Application.DTOs;
using Application.Queries.TreatmentQueries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers.TreatmentQueryHandlers
{
    public class GetAllTreatmentsQueryHandler : IRequestHandler<GetAllTreatmentsQuery, List<TreatmentDto>>
    {
        private readonly ITreatmentRepository repository;
        private readonly IMapper mapper;

        public GetAllTreatmentsQueryHandler(ITreatmentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<List<TreatmentDto>> Handle(GetAllTreatmentsQuery request, CancellationToken cancellationToken)
        {
            var treatments = await repository.GetAllAsync();
            return mapper.Map<List<TreatmentDto>>(treatments);
        }
    }
}

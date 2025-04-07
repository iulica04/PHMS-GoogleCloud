using Application.DTOs;
using Application.Queries.TreatmentQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers.TreatmentQueryHandlers
{
    public class GetTreatmentByIdQueryHandler : IRequestHandler<GetTreatmentByIdQuery, Result<TreatmentDto>>
    {
        private readonly ITreatmentRepository repository;
        private readonly IMapper mapper;
        public GetTreatmentByIdQueryHandler(ITreatmentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<TreatmentDto>> Handle(GetTreatmentByIdQuery request, CancellationToken cancellationToken)
        {
            var treatment = await repository.GetByIdAsync(request.TreatmentId);
            if (treatment == null)
            {
                return Result<TreatmentDto>.Failure("Treatment not found");
            }
            return Result<TreatmentDto>.Success(mapper.Map<TreatmentDto>(treatment));
        }
    }
}

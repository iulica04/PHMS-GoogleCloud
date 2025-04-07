using Application.DTOs;
using Application.Queries.MedicQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers.MedicQueryHandlers
{
    public class GetMedicByIdQueryHandler : IRequestHandler<GetMedicByIdQuery, Result<MedicDto>>
    {
        private readonly IMedicRepository repository;
        private readonly IMapper mapper;
        public GetMedicByIdQueryHandler(IMedicRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<MedicDto>> Handle(GetMedicByIdQuery request, CancellationToken cancellationToken)
        {
            var medic = await repository.GetByIdAsync(request.Id);
            if (medic == null)
            {
                return Result<MedicDto>.Failure($"Medic with id {request.Id} not found");
            }
            return Result<MedicDto>.Success(mapper.Map<MedicDto>(medic));
        }
    }
}

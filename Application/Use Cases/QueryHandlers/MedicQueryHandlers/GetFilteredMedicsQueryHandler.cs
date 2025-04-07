using Application.DTOs;
using Application.Queries;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Gridify;

namespace Application.QueryHandlers.MedicQueryHandlers
{
    public class GetFilteredMedicsQueryHandler : IRequestHandler<GetFilteredQuery<Medic, MedicDto>, Result<PagedResult<MedicDto>>>
    {
        private readonly IMedicRepository repository;
        private readonly IMapper mapper;
        public GetFilteredMedicsQueryHandler(IMedicRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<PagedResult<MedicDto>>> Handle(GetFilteredQuery<Medic, MedicDto> request, CancellationToken cancellationToken)
        {
            var medics = await repository.GetAllAsync();
            if (request.Filter != null)
            {
                medics = medics.AsQueryable().Where(request.Filter);
            }

            var totalCount = medics.Count();
            var pagedMedics = medics.AsQueryable().ApplyPaging(request.Page, request.PageSize);

            var medicDtos = mapper.Map<List<MedicDto>>(pagedMedics);
            var pagedResult = new PagedResult<MedicDto>(medicDtos, totalCount);

            return Result<PagedResult<MedicDto>>.Success(pagedResult);
        }
    }
}

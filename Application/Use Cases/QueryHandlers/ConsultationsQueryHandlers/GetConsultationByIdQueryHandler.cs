using Application.DTOs;
using Application.Use_Cases.Queries.ConsultationsQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.ConsultationsQueryHandlers
{
    public class GetConsultationByIdQueryHandler : IRequestHandler<GetConsultationByIdQuery, Result<ConsultationDto>>
    {
        private readonly IConsultationRepository repository;
        private readonly IMapper mapper;

        public GetConsultationByIdQueryHandler(IConsultationRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        public async Task<Result<ConsultationDto>> Handle(GetConsultationByIdQuery request, CancellationToken cancellationToken)
        {
            var consultation = await repository.GetByIdAsync(request.Id);
            if (consultation == null)
            {
                return Result<ConsultationDto>.Failure($"Consultation with id {request.Id} not found");
            }
            return Result<ConsultationDto>.Success(mapper.Map<ConsultationDto>(consultation));
        }
    }
}

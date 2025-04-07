using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Queries.ConsultationsQueries
{
    public class GetConsultationByIdQuery : IRequest<Result<ConsultationDto>>
    {
        public Guid Id { get; set; }
    }
}

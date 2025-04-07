using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries.ConsultationsQueries
{
    public class GetAllConsultationsQuery : IRequest<List<ConsultationDto>>
    {
    }
}

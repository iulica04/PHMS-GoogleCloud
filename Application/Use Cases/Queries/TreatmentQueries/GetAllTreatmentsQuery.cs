using Application.DTOs;
using MediatR;

namespace Application.Queries.TreatmentQueries
{
    public class GetAllTreatmentsQuery : IRequest<List<TreatmentDto>>
    {
    }
}

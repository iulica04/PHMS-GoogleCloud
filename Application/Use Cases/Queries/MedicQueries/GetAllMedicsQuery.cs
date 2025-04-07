using Application.DTOs;
using MediatR;

namespace Application.Queries.MedicQueries
{
    public class GetAllMedicsQuery : IRequest<List<MedicDto>>
    {
    }
}

using Application.DTOs;
using MediatR;

namespace Application.Queries.PatientQueries
{
    public class GetAllPatientsQuery : IRequest<List<PatientDto>>
    {
    }
}

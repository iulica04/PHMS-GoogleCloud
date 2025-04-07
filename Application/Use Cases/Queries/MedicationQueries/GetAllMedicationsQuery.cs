using Application.DTOs;
using MediatR;

namespace Application.Queries.MedicationQueries
{
    public class GetAllMedicationsQuery : IRequest<List<MedicationDto>>
    {
    }
}

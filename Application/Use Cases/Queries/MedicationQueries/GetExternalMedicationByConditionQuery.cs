using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries.MedicationQueries
{
    public class GetExternalMedicationByConditionQuery : IRequest<List<ExternalMedicationDto>>
    {
        public required string Condition { get; set; }
    }
}

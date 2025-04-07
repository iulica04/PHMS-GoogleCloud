using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Queries.MedicationQueries
{
    public class GetMedicationByIdQuery : IRequest<Result<MedicationDto>>
    {
        public Guid MedicationId { get; set; }
    }
}

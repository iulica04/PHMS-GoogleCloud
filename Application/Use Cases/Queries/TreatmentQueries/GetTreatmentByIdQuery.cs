using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Queries.TreatmentQueries
{
    public class GetTreatmentByIdQuery : IRequest<Result<TreatmentDto>>
    {
        public Guid TreatmentId { get; set; }
    }
}

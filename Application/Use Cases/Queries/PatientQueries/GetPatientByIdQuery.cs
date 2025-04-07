using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Queries.PatientQueries
{
    public class GetPatientByIdQuery : IRequest<Result<PatientDto>>
    {
        public Guid Id { get; set; }
    }
}

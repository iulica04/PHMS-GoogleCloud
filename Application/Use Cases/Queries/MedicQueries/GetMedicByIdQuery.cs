using Application.DTOs;
using Domain.Common;
using MediatR;


namespace Application.Queries.MedicQueries
{
    public class GetMedicByIdQuery : IRequest<Result<MedicDto>>
    {
        public Guid Id { get; set; }
    }
}

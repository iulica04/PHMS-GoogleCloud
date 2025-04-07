using Application.DTOs;
using MediatR;

namespace Application.Queries.MedialConditionQueries
{
    public class GetMedicalConditionByIdQuery : IRequest<MedicalConditionDto>
    {
        public Guid PatientId { get; set; }
        public Guid MedicalConditionId { get; set; }
        public GetMedicalConditionByIdQuery(Guid patientId, Guid id)
        {
            PatientId = patientId;
            MedicalConditionId = id;
        }
    }
}

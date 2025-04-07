using Application.DTOs;
using MediatR;

namespace Application.Queries.MedialConditionQueries
{
    public class GetAllMedicalConditionsQuery : IRequest<List<MedicalConditionDto>>
    {
        public Guid PatientId { get; set; }
        public GetAllMedicalConditionsQuery(Guid patientId)
        {
            PatientId = patientId;
        }
    }
}

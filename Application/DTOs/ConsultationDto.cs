using Domain.Entities;

namespace Application.DTOs
{
    public class ConsultationDto
    {
        public Guid Id { get; set; }
        public Guid MedicId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public ConsultationStatus Status { get; set; }


    }
}

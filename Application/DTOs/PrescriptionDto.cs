using Domain.Entities;

namespace Application.DTOs
{
    public class PrescriptionDto
    {
        public Guid Id { get; set; }
        public Guid TreatmentId { get; set; }
        public DateTime DateIssued { get; set; }
        public required List<Medication> Medications { get; set; }
    }
}

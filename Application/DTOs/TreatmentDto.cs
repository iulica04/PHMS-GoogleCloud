using Domain.Enums;

namespace Application.DTOs
{
    public class TreatmentDto
    {
        public Guid TreatmentId { get; set; }
        public required string Name { get; set; }
        public TreatmentType Type { get; set; }
        public required string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Duration { get; set; }
        public required List<MedicationDto> Medications { get; set; }
    }
}

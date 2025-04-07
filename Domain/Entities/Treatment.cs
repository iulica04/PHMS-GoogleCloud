using Domain.Enums;

namespace Domain.Entities
{
    public class Treatment
    {
        public Guid TreatmentId { get; set; }
        public Guid MedicalConditionId { get; set; }
        public TreatmentType Type { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Duration { get; set; }
        public required List<Medication> Medications { get; set; }
    }
}

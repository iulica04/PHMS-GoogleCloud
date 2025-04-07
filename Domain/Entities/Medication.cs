using Domain.Enums;

namespace Domain.Entities
{
    public class Medication
    {
        public Guid Id { get; set; }
        public Guid TreatmentId { get; set; }
        public required string Name { get; set; }

        public MedicationType Type { get; set; }
        public required string Dosage { get; set; }
        public required string Administration { get; set; }
        public required string Ingredients { get; set; }
        public required string AdverseEffects { get; set; }
    }
}

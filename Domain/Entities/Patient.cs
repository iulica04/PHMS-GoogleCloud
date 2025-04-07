namespace Domain.Entities
{
    public class Patient : User
    {
        public required List<MedicalCondition> MedicalConditions{ get; set; }
        public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    }
}

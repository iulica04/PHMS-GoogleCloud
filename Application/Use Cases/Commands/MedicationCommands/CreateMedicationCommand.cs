using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Commands.MedicationCommand
{
    public class CreateMedicationCommand : IRequest<Result<Guid>>
    {
        public Guid TreatmentId { get; set; }
        public required string Name { get; set; }
        public MedicationType Type { get; set; }
        public required string Dosage { get; set; }
        public required string Administration { get; set; }
        public required string Ingredients { get; set; }
        public required string AdverseEffects { get; set; }
    }
}
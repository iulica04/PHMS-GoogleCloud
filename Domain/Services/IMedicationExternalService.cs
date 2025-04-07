using Domain.Entities;

namespace Domain.Services
{
    public interface IMedicationExternalService
    {
        Task<List<ExternalMedication>> GetMedicationsByConditionAsync(string condition);
    }
}

using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMedicationRepository
    {
        Task<IEnumerable<Medication>> GetAllAsync();
        Task<Medication?> GetByIdAsync(Guid id);
        Task<Result<Guid>> AddAsync(Medication medication);
        Task UpdateAsync(Medication medication);
        Task DeleteAsync(Guid id);
    }
}
